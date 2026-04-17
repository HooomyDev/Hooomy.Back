using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class RequestRepository(HooomeDbContext dbContext) 
    : BaseRepository<Request>(dbContext), IRequestRepository
{
    public async Task<Request?> GetByIdAndUserId(Guid requestId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Address)     
            .Include(r => r.Images)       
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

        if (status.HasValue && status.Value != RequestStatus.Unknown)
            query = query.Where(r => r.Status == status.Value);

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<string, int>> GetRequestsCount(
        DateTime startDate,
        DateTime endDate,
        StatisticGroupType groupType,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Requests
            .Where(r => r.CreatedAt.Date >= startDate.Date &&
                       r.CreatedAt.Date <= endDate.Date &&
                       !r.IsDeleted);

        return groupType switch
        {
            StatisticGroupType.Day => await query
                                .GroupBy(r => r.CreatedAt.Date)
                                .Select(g => new { Key = g.Key.ToString("yyyy-MM-dd"), Count = g.Count() })
                                .ToDictionaryAsync(g => g.Key, g => g.Count, cancellationToken),
            StatisticGroupType.Month => await query
                                .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
                                .Select(g => new { Key = $"{g.Key.Year}-{g.Key.Month:D2}", Count = g.Count() })
                                .ToDictionaryAsync(g => g.Key, g => g.Count, cancellationToken),
            StatisticGroupType.Year => await query
                                .GroupBy(r => r.CreatedAt.Year)
                                .Select(g => new { Key = g.Key.ToString(), Count = g.Count() })
                                .ToDictionaryAsync(g => g.Key, g => g.Count, cancellationToken),
            _ => throw new ArgumentException("Invalid group type"),
        };
    }
    
    public async Task<(IEnumerable<Request> Items, int TotalCount)> GetRequestsWithPagination(
        string? title = null, 
        RequestStatus? status = null, 
        RequestCategory? category = null, 
        int page = 1, 
        int pageSize = 10, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Requests
             .Include(r => r.Address)
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

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public override async Task Delete(Request entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        foreach(var image in entity.Images)
        {
            image.IsDeleted = true;
            image.DeletedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}