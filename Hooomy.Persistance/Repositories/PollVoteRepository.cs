using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class PollVoteRepository(HooomeDbContext dbContext)
    : BaseRepository<PollVote>(dbContext), IPollVoteRepository
{
    public async Task<bool> IsVoteExist(Guid pollId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(pv => pv.PollId == pollId && pv.UserId == userId, cancellationToken);
    }
}