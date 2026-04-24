using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class MinioOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public string ExternalEndpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public bool UseSSL { get; set; }
    public Dictionary<ImageType, BucketConfig> Buckets { get; set; } = [];
}