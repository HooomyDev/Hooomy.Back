using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class RequestCommentRepository(HooomeDbContext dbContext) 
    : BaseRepository<RequestComment>(dbContext), IRequestCommentRepository
{
    public async Task<(ICollection<RequestComment> items, int totalCount)> GetByRequestId(string? text, RequestCommentStatus? status, Guid? requestId, int page = 1, int pageSize = 5, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(rc => rc.Company)
            .Include(rc => rc.Images)
            .AsQueryable();

        if(requestId is not null)
        {
            query = query.Where(rc => rc.RequestId == requestId && !rc.IsDeleted);
        }

        if(!string.IsNullOrWhiteSpace(text))
        {
            query = query.Where(rc => rc.Text.Contains(text));
        }

        if(status is not null)
        {
            query = query.Where(rc => rc.Status == status);
        }

        var items = await query
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            
            .ToListAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        return (items, totalCount);
    }

    public override async Task<RequestComment?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(rc => rc.Company)
            .Include(rc => rc.Images)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync(rc => rc.Id == id, cancellationToken);
    }

    public override async Task Delete(RequestComment entity, CancellationToken cancellationToken = default)
    {
        entity.Status = RequestCommentStatus.Deleted;
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        foreach(var image in entity.Images)
        {
            image.IsDeleted = true;
            image.DeletedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<int> CountWithFilter(Guid? requestId = null, string? filter = "all", CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if(requestId is not null)
        {
            query = query.Where(rc => rc.RequestId == requestId);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            query = filter.ToLower() switch
            {
                "active" => query.Where(rc => !rc.IsDeleted),        
                "deleted" => query.Where(rc => rc.IsDeleted),      
                "all" => query,                                      
                _ => query
            };
        }

        return query.CountAsync(cancellationToken);
    }
}
