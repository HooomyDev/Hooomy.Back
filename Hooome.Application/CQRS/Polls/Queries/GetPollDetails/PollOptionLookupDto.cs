namespace Hooome.Application.CQRS.Polls.Queries.GetPollDetails;

public class PollOptionLookupDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public int VoteCount { get; set; }
}