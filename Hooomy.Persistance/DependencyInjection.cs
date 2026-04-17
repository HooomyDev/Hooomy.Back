using Hooome.Application.Interfaces;
using Hooome.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hooome.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<HooomeDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(HooomeDbContext)));
        });

        services.AddScoped<IHooomeDbContext>(provider =>
            provider.GetService<HooomeDbContext>()
                ?? throw new NullReferenceException("Provider cant't be null"));

        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IRequestRepository, RequestRepository>();

        return services;
    }
}