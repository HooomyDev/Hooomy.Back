using Hooome.WebApi.Hubs;

namespace Hooome.WebApi.Configuration;

public static class EndpointsExtensions
{
    public static void UseApiEndpoints(this WebApplication app)
    {
        app.MapControllers();
        app.MapHub<ChatHub>("/chat-hub");
    }
}
