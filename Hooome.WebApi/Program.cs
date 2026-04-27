using Hooome.Application;
using Hooome.Persistance;
using Hooome.WebApi.Configuration;
using Hooome.WebApi.Hubs;
using Hooome.WebApi.Middleware;
using Hooome.WebApi.Services;
using Microsoft.Extensions.FileProviders;
using Serilog;

Log.Logger = SerilogConfigurator.Configure();

try
{
    Log.Information("Starting Hooome Web API");

    var builder = WebApplication.CreateBuilder(args);

    // environment
    builder.Environment.ConfigureWebRootPath(builder.Configuration);
    builder.Host.UseSerilog();

    // base services
    builder.Services.AddRedis(builder.Configuration);
    builder.Services.AddSignalR();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAutoMapperProfiles();
    builder.Services.AddSwagger();

    // layers
    builder.Services.AddApplication();
    builder.Services.AddPersistence(builder.Configuration);

    // auth
    builder.Services.AddApiCorsPolicies();
    builder.Services.AddApiAuthentication();
    builder.Services.AddApiAuthorizationPolicies();

    // services
    builder.Services.ConfigureForm();
    builder.Services.AddServices(builder.Configuration);

    var app = builder.Build();

    // init db
    await app.InitializeDatabase();

    // swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerDocumentation();
    }

    // middlevare pipeline
    app.UseApiExceptionHandler();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors();

    // auth
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSerilog();
    app.UseApiEndpoints();
    app.UseCustomStaticFiles();

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