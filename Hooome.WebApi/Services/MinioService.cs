using Hooome.Application.Interfaces;
using Hooome.Domain.Enums;
using Hooome.WebApi.Models;
using Minio;
using Minio.DataModel.Args;
using Serilog;

namespace Hooome.WebApi.Services;

public class MinioService(IMinioClient minioClient,
    Dictionary<ImageType, BucketConfig> buckets,
    IConfiguration configuration)
    : IMinioService
{
    public async Task<string> UploadImage(IFormFile file, ImageType type, Guid entityId)
    {
        try
        {
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
            var bucket = GetBucket(type);

            var args = new RemoveObjectArgs()
                .WithBucket(bucket.Name)
                .WithObject(objectName);

            await minioClient.RemoveObjectAsync(args);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to delete {ImageName} from {ImageType}", objectName, type);
            throw;
        }
    }

    public string GetUrl(ImageType type, string imageName)
    {
        var bucket = GetBucket(type);

        var endpoint = configuration["MinIO:ExternalEndpoint"];
        var useSSL = bool.Parse(configuration["MinIO:UseSSL"] ?? "false");
        var protocol = useSSL ? "https" : "http";

        return $"{protocol}://{endpoint}/{bucket.Name}/{imageName}";
    }

    private async Task EnsureBucketExist(string bucketName)
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

    private BucketConfig GetBucket(ImageType imageType)
    {
        if (!buckets.TryGetValue(imageType, out var bucket))
            throw new ArgumentException($"Configuration for {imageType} not found");

        return bucket;
    }
}
