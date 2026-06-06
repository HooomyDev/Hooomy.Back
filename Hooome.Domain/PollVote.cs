namespace Hooome.Domain;

public class PollVote
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Guid PollId { get; set; }
    public Poll Poll { get; set; } = null!;

    public Guid OptionId { get; set; }
    public PollOption Option { get; set; } = null!;
}