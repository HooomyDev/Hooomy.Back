namespace Hooome.Domain;

public class Inquiry
{
    public Guid Id { get; set; }
    public string Message { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}