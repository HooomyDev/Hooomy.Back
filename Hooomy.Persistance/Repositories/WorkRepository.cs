using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class WorkRepository(HooomeDbContext dbContext)
    : BaseRepository<Work>(dbContext), IWorkRepository
{
    public async Task<ICollection<Work>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(w => w.Address)
                .ThenInclude(a => a.FavoriteAddresses)
            .Where(w => w.Address.FavoriteAddresses.Any(fa => fa.UserID == userId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
