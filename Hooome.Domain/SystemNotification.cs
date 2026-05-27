namespace Hooome.Domain;

public class SystemNotification
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
