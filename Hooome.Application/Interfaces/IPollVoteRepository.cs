using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IPollVoteRepository : IRepository<PollVote>
{
    Task<bool> IsVoteExist(Guid pollId, Guid userId, CancellationToken cancellationToken = default);
}