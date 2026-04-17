namespace Hooome.Domain;

public class RequestImage
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public long FileSize { get; set; }
    public bool IsMain { get; set; }
    public Guid RequestId { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public Request Request { get; set; } = null!;
}
