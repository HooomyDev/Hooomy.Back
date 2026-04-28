using Hooome.Application.Interfaces;
using Hooome.Domain.Enums;
using Hooome.WebApi.Models;
using Hooome.WebApi.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Minio;

namespace Hooome.WebApi.Configuration;

public static class ServicesExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DataSeeder>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IMapClusteringService, MapClusteringService>();
        services.AddScoped<IChatModerationService, ChatModerationService>();

        services.Configure<MinioOptions>(configuration.GetSection("MinIO"));

        services.AddSingleton<IMinioClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<MinioOptions>>().Value;

            return new MinioClient()
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(options.UseSSL)
                .Build();
        });

        services.AddSingleton(sp =>
        {
            var buckets = configuration.GetSection("MinIO:Buckets")
                .Get<Dictionary<ImageType, BucketConfig>>();

            return buckets ?? [];
        });

        services.AddScoped<IMinioService, MinioService>();
    }

    public static void ConfigureForm(this IServiceCollection services)
    {
        services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = 100 * 1024 * 1024;
            options.MultipartHeadersLengthLimit = int.MaxValue;
        });
    }

    public static void UseCustomStaticFiles(this WebApplication app)
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "static")),
        });
    }
}