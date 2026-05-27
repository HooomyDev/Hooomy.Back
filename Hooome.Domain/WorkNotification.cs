namespace Hooome.Domain;

public class WorkNotification
{
    public Guid Id { get; set; }
    public Guid WorkId { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Work Work { get; set; } = null!;
}
