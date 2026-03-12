namespace Hooome.Domain;

public class PollOption
{
    public Guid Id { get; set; }
    public Guid PollId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}
