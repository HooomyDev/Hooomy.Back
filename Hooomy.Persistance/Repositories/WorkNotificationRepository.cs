using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class WorkNotificationRepository(HooomeDbContext dbContext)
    : BaseRepository<WorkNotification>(dbContext), IWorkNotificationRepository
{
    public async Task<IEnumerable<WorkNotification>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(wn => wn.Work)
                .ThenInclude(w => w.Address)
                    .ThenInclude(a => a.FavoriteAddresses)
            .Where(wn => wn.Work.Address.FavoriteAddresses.Any(fa => fa.UserId == userId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
