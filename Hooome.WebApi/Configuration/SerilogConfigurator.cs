using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Runtime.CompilerServices;

namespace Hooome.WebApi.Configuration;

public static class SerilogConfigurator
{
    public static Logger Configure()
    {
        return new LoggerConfiguration()
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
    }

    public static void UseSerilog(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("User", httpContext.User?.Identity?.Name ?? "anonymous");
                diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent);
            };
        });
    }
}

