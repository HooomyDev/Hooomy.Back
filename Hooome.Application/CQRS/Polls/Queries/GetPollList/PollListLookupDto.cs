using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollList;

public class PollListLookupDto 
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public int VoteCount { get; set; }
    public bool IsActive { get; set; }
    public PollType Type { get; set; }
}
