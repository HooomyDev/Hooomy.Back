namespace Hooome.Domain;

public class CompanyImage
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = null!;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
}