using Hooome.WebApi.Models.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Hooome.WebApi.Configuration;

public static class AuthApiExtensions
{
    public static void AddApiAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "http://hooome-identity:8080"; 
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
    }

    public static void AddApiAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy =>
            {
                policy.RequireRole("Admin");
            })
            .AddPolicy("EmployeeOnly", policy =>
            {
                policy.RequireRole("Employee");
            })
            .AddPolicy("ApprovedOnly", policy =>
            {
                policy.RequireAssertion(context =>
                {
                    if (!context.User.Identity.IsAuthenticated)
                        return false;

                    var statusClaim = context.User.FindFirst("status")?.Value;

                    if (string.IsNullOrEmpty(statusClaim))
                        return false;

                    var status = Enum.Parse<UserStatus>(statusClaim);

                    return status == UserStatus.Approved;
                });
            })
            .AddPolicy("UserPendingOrGuest", policy =>
            {
                policy.RequireAssertion(context =>
                {
                    if (!context.User.Identity.IsAuthenticated)
                        return true;

                    var statusClaim = context.User.FindFirst("status")?.Value;

                    if (string.IsNullOrEmpty(statusClaim))
                        return true;

                    var status = Enum.Parse<UserStatus>(statusClaim);

                    if (status == UserStatus.Approved)
                        return true;

                    return status == UserStatus.Pending;
                });
            });
    }
}