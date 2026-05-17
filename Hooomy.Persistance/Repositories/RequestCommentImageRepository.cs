using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class RequestCommentImageRepository(HooomeDbContext dbContext)
    : BaseRepository<RequestCommentImage>(dbContext), IRequestCommentImageRepository
{
    public async Task<ICollection<RequestCommentImage>> GetByCommentId(Guid commentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(rci => rci.RequestCommentId == commentId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}