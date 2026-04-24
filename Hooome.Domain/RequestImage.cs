namespace Hooome.Domain;

public class RequestImage
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public string FileName { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = null!;
    public DateTime UploadedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public Request Request { get; set; } = null!;
}
