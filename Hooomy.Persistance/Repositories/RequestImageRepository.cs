using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class RequestImageRepository(HooomeDbContext dbContext)
    : BaseRepository<RequestImage>(dbContext), IRequestImageRepository
{
    public async Task<IEnumerable<RequestImage>> GetAllByRequestId(Guid requestId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(i => i.RequestId == requestId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
