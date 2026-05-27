using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class RequestNotificationRepository(HooomeDbContext dbContext)
    : BaseRepository<RequestNotification>(dbContext), IRequestNotificationRepository
{
    public async Task<IEnumerable<RequestNotification>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rn => rn.Request)
            .Where(rn => rn.Request.UserId == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}