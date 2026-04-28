namespace Hooome.WebApi.Configuration;

public static class EnvironmentExtensions
{
    public static void ConfigureWebRootPath(this IWebHostEnvironment environment, IConfiguration configuration)
    {
        var webRootPath = configuration["WebRootPath"];
        environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), webRootPath ?? "static");
    }
}
