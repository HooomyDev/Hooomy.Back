using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class RequestRepository(HooomeDbContext dbContext)
    : BaseRepository<Request>(dbContext), IRequestRepository
{
    public override async Task<Request?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Address)
            .Include(r => r.Images)
            .Include(r => r.Comments)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Request?> GetByIdAndUserId(Guid requestId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Address)
            .Include(r => r.Images)
            .Include(r => r.Comments)
            .FirstOrDefaultAsync(x => x.UserID == userId && x.Id == requestId, cancellationToken);
    }

    public async Task<IEnumerable<Request>> GetFilteredRequests(
        Guid userId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        RequestStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(r => r.Address)
            .Include(r => r.Images)
            .Where(r => !r.IsDeleted && r.UserID == userId)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(r => r.CreatedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(r => r.CreatedAt <= endDate.Value.Date.AddDays(1));

        if (status != RequestStatus.Unknown)
            query = query.Where(r => r.Status == status);

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetRequestsByDate(
        DateTime startDate,
        DateTime endDate,
        Guid? companyId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Requests
            .Include(r => r.Address)
            .Where(r => r.CreatedAt.Date >= startDate.Date &&
                       r.CreatedAt.Date <= endDate.Date &&
                       !r.IsDeleted && !r.IsDeleted);

        if(companyId.HasValue)
        {
            query = query.Where(r => r.Address.ServicedByCompanyId == companyId);
        }

        return await query
            .GroupBy(r => r.CreatedAt.Date)
            .Select(g => new { Key = g.Key.ToString("yyyy-MM-dd"), Count = g.Count() })
            .AsNoTracking()
            .ToDictionaryAsync(g => g.Key, g => g.Count, cancellationToken); 
    }

    public async Task<(IEnumerable<Request> Items, int TotalCount)> GetRequestsWithPagination(
        string? title = null,
        Guid? companyId = null,
        RequestStatus? status = null,
        RequestCategory? category = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Requests
             .Include(r => r.Address)
             .Include(r => r.Comments)
             .Where(r => !r.IsDeleted)
             .AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(r => r.Title.Contains(title));
        }

        if (status.HasValue && status.Value != RequestStatus.Unknown)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        if (category.HasValue && category.Value != RequestCategory.None)
        {
            query = query.Where(r => r.Category == category.Value);
        }

        if (companyId is not null)
        {
            query = query.Where(r => r.Address.ServicedByCompanyId == companyId);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public override async Task Delete(Request entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        foreach (var image in entity.Images)
        {
            image.IsDeleted = true;
            image.DeletedAt = DateTime.UtcNow;
        }

        foreach (var comment in entity.Comments)
        {
            comment.Status = RequestCommentStatus.Deleted;
            comment.IsDeleted = true;
            comment.DeletedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}