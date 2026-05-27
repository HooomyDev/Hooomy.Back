namespace Hooome.Domain;

public class RequestNotification
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Request Request { get; set; } = null!;
}
