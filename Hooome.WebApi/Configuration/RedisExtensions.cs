namespace Hooome.WebApi.Configuration;

public static class RedisExtensions
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            var connection = configuration.GetConnectionString("Redis");
            options.Configuration = connection;
        });
    }
}