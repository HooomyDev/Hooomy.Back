namespace Hooome.Domain;

public class RequestCommentImage 
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    public Guid CommentId { get; set; }
    public RequestComment? Comment { get; set; }
}