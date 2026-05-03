namespace Hooome.Domain;

public class RequestComment
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }
    public string SenderName { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string PhotoUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public Request Request { get; set; } = null!;
}
