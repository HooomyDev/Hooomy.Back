using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollDetails;

public class PollDetailsVm
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public int VoteCount { get; set; }
    public bool IsActive { get; set; }
    public PollType Type { get; set; }
    public IList<PollOptionLookupDto> Options { get; set; } = [];
    public IList<Guid>? UserVotes { get; set; }
    public bool UserHasVoted => UserVotes != null && UserVotes.Any(x => x != Guid.Empty);
}
