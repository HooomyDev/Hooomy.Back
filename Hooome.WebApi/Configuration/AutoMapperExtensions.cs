using Hooome.Application.Common.Mappings;
using Hooome.Application.Interfaces;
using System.Reflection;

namespace Hooome.WebApi.Configuration;

public static class AutoMapperExtensions
{
    public static void AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(IHooomeDbContext).Assembly));
        });

    }
}
