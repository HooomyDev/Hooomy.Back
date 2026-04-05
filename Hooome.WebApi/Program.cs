using Hooome.Application;
using Hooome.Application.Common.Mappings;
using Hooome.Application.Interfaces;
using Hooome.Persistance;
using Hooome.WebApi.Hubs;
using Hooome.WebApi.Middleware;
using Hooome.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;

// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} " +
                       "{Properties:j}{NewLine}{Exception}")
    .WriteTo.File("logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] " +
                       "{Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting Hooome Web API");

    var builder = WebApplication.CreateBuilder(args);

    var webRootPath = builder.Configuration["WebRootPath"];
    builder.Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), webRootPath ?? "static");

    // Добавляем Serilog
    builder.Host.UseSerilog();

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        var connection = builder.Configuration.GetConnectionString("Redis");
        options.Configuration = connection;
    });

    builder.Services.AddSignalR();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // AutoMapper
    builder.Services.AddAutoMapper(config =>
    {
        config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        config.AddProfile(new AssemblyMappingProfile(typeof(IHooomeDbContext).Assembly));
    });

    // Application и Persistence слои
    builder.Services.AddApplication();
    builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddScoped<DataSeeder>();

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("FrontendPolicy", policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://hooome-identity:8080"; // Use service name in Docker
        options.Audience = "HooomeWebApi";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = "HooomeWebApi",
            ValidateIssuer = true,
            ValidIssuer = "http://hooome-identity:8080",
            ValidateLifetime = true
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/chat-hub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Log.Error("Authentication failed: {Error}", context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Log.Debug("Token validated for {User}", context.Principal?.Identity?.Name);
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
            }
        };
    });

    // Swagger с поддержкой JWT
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Hooome API",
            Version = "v1",
            Description = "Hooome Application API",
            Contact = new OpenApiContact
            {
                Name = "Hooome Team",
                Email = "support@hooome.com"
            }
        });

        // Добавляем поддержку JWT в Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

    builder.Services.Configure<FormOptions>(options =>
    {
        options.ValueLengthLimit = int.MaxValue;
        options.MultipartBodyLengthLimit = 100 * 1024 * 1024; 
        options.MultipartHeadersLengthLimit = int.MaxValue;
    });

    // Current User Service
    builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
    builder.Services.AddHttpContextAccessor();

    // Image Service
    builder.Services.AddScoped<IImageService, ImageService>();

    var app = builder.Build();

    // Инициализация базы данных
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<HooomeDbContext>();
        DbInitializer.Initialize(context);

        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAllDataAsync(context);
    }

    // Swagger - всегда включен для разработки
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.RoutePrefix = string.Empty;
            config.SwaggerEndpoint("/swagger/v1/swagger.json", "Hooome API v1");
            config.DocumentTitle = "Hooome API Documentation";
        });
    }

    // Middleware pipeline
    app.UseCustomExceptionHandler();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("FrontendPolicy");

    // Важно: Authentication должен быть перед Authorization!
    app.UseAuthentication();
    app.UseAuthorization();

    // Логирование HTTP запросов (заменяет app.UseSerilog())
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("User", httpContext.User?.Identity?.Name ?? "anonymous");
            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"]);
        };
    });

    app.MapControllers();
    app.MapHub<ChatHub>("/chat-hub");

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "static")),
    });

    Log.Information("Hooome API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Hooome API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}