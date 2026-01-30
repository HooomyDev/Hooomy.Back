using Hooome.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hooome.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];
        var serverVersion = new MySqlServerVersion(new Version(configuration["DatabaseSettings:ServerVersion"]
            ?? throw new NullReferenceException("server version was null")));

        services.AddDbContext<HooomeDbContext>(options =>
        {
            options.UseMySql(
                connectionString, 
                serverVersion, 
                mySqlOptions => mySqlOptions.EnableStringComparisonTranslations()
            );
        });

        services.AddScoped<IHooomeDbContext>(provider =>
            provider.GetService<HooomeDbContext>()
                ?? throw new NullReferenceException("provider cant't be null"));

        return services;
    }
}