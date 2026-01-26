using AutoMapper;
using System.Reflection;

namespace Hooome.Application.Common.Mappings;

/// <summary>
/// AutoMapper profile for automatic registration of mappings from an assembly.
/// 
/// This class automatically discovers and registers all classes that implement
/// the IMapWith<> interface, and invokes their Mapping method to configure mappings.
/// </summary>
public class AssemblyMappingProfile : Profile
{
    /// <param name="assembly">Assembly to scan for classes with mapping configurations.</param>
    public AssemblyMappingProfile(Assembly assembly)
        => ApplyMappingsFromAssembly(assembly);

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        // 1. Find all public types that implemet IMapWith<T> interface in the assembly
        var types = assembly.GetExportedTypes()
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
            .ToList();

        // 2. For each found type, register its mappings
        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            
            var methodInfo = type.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}