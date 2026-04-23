using Hooome.Application.Interfaces;
using Hooome.Domain.Enums;
using Hooome.WebApi.Models;
using Microsoft.Extensions.Caching.Distributed;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Serilog;

namespace Hooome.WebApi.Services;

public class MinioService(
    IMinioClient minioClient,
    IDistributedCache cache,
    Dictionary<ImageType, BucketConfig> buckets,
    IConfiguration configuration)
    : IMinioService
{
    public async Task<string> UploadImage(IFormFile file, ImageType type, Guid entityId)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(file);

            var bucket = GetBucket(type);

            await EnsureBucketExist(bucket.Name);

            var extension = Path.GetExtension(file.FileName);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var objectName = $"{entityId}_{timestamp}{extension}";

            using var stream = file.OpenReadStream();
            var args = new PutObjectArgs()
                .WithBucket(bucket.Name)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType);

            await minioClient.PutObjectAsync(args);

            Log.Information("Uploaded {ObjectName} to {BucketName} ({Size} bytes)",
                objectName, bucket.Name, file.Length);

            return objectName;
        }
        catch (ArgumentNullException ex)
        {
            Log.Error(ex, "File {FileName} is null", file.FileName);
            throw;
        }
        catch (AccessDeniedException ex)
        {
            Log.Error(ex, "Access denied");
            throw;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to upload image for {ImageType} {EntityId}", type, entityId);
            throw;
        }
    }

    public async Task DeleteImage(ImageType type, string objectName)
    {
        try
        {
            if (string.IsNullOrEmpty(objectName))
            {
                throw new ArgumentNullException(nameof(objectName), "File name is required");
            }

            var bucket = GetBucket(type);

            var args = new RemoveObjectArgs()
                .WithBucket(bucket.Name)
                .WithObject(objectName);

            await minioClient.RemoveObjectAsync(args);

            var cacheKey = $"minio_url_{type}_{objectName}";
            await cache.RemoveAsync(cacheKey);
        }
        catch (ArgumentNullException ex)
        {
            Log.Error(ex, "Invalid parameter for delete operation");
            throw;
        }
        catch (BucketNotFoundException ex)
        {
            Log.Error(ex, "Bucket not found when deleting {ObjectName}", objectName);
            throw;
        }
        catch (AccessDeniedException ex)
        {
            Log.Error(ex, "Access denied");
            throw;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to delete {ImageName} from {ImageType}", objectName, type);
            throw;
        }
    }

    public async Task<string> GetUrl(ImageType type, string objectName)
    {
        try
        {
            if (string.IsNullOrEmpty(objectName))
            {
                throw new ArgumentNullException(nameof(objectName), "File name is required");
            }

            var cacheKey = $"minio_url_{type}_{objectName}";

            var cachedUrl = await cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedUrl))
            {
                return cachedUrl;
            }

            var bucket = GetBucket(type);

            var endpoint = configuration["MinIO:ExternalEndpoint"];
            var useSSL = bool.Parse(configuration["MinIO:UseSSL"] ?? "false");
            var protocol = useSSL ? "https" : "http";

            var url = $"{protocol}://{endpoint}/{bucket.Name}/{objectName}";

            await cache.SetStringAsync(cacheKey, url, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return url;
        }
        catch (ArgumentNullException ex)
        {
            Log.Error(ex, "Argument null error in GetUrl for {ImageType}", type);
            throw;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error in GetUrl for {ImageType} {ObjectName}", type, objectName);
            throw;
        }
    }

    private async Task EnsureBucketExist(string bucketName)
    {
        try
        {
            var isExists = await minioClient
                .BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));

            if (!isExists)
            {
                await minioClient
                    .MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));

                var policy = $@"{{
                ""Version"": ""2012-10-17"",
                ""Statement"": [
                    {{
                        ""Effect"": ""Allow"",
                        ""Principal"": {{ ""AWS"": [""*""] }},
                        ""Action"": [ ""s3:GetObject"" ],
                        ""Resource"": [ ""arn:aws:s3:::{bucketName}/*"" ]
                    }}
                ]
            }}";

                var args = new SetPolicyArgs()
                    .WithBucket(bucketName)
                    .WithPolicy(policy);

                await minioClient.SetPolicyAsync(args);
            }
        }
        catch (BucketNotFoundException ex)
        {
            Log.Error(ex, "Bucket {bucketName} not found", bucketName);
            throw;
        }
    }

    private BucketConfig GetBucket(ImageType imageType)
    {
        if (!buckets.TryGetValue(imageType, out var bucket))
            throw new ArgumentException($"Configuration for bucket \"{imageType}\" not found");

        return bucket;
    }
}
