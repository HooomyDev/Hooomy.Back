namespace Hooome.Domain;

public class PollOption
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Guid PollId { get; set; }
    public Poll Poll { get; set; } = null!;

    public ICollection<PollVote> Votes { get; set; } = [];
}