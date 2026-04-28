using Hooome.Persistance;
using Hooome.WebApi.Services;

namespace Hooome.WebApi.Configuration;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<HooomeDbContext>();
        DbInitializer.Initialize(context);

        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAllDataAsync(context);
    }
}
