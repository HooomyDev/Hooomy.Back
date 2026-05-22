using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class RequestReviewRepository(HooomeDbContext dbContext)
    : BaseRepository<RequestReview>(dbContext), IRequestReviewRepository
{
    public async Task<ICollection<RequestReview>?> GetByCompanyId(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rr => rr.Request)
                .ThenInclude(r => r.Address)
            .Where(rr => !rr.IsDeleted && rr.Request.Address.ServicedByCompanyId == companyId)
            .OrderByDescending(rr => rr.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<RequestReview?> GetByRequestId(Guid requestId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(rw => rw.RequestId == requestId, cancellationToken);
    }
}
