using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class WorkRepository(HooomeDbContext dbContext)
    : BaseRepository<Work>(dbContext), IWorkRepository
{
    public async Task<(ICollection<Work> Items, int TotalCount)> GetAllWithPagination(
        int page = 1,
        int pageSize = 10,
        RequestCategory? category = null,
        WorkSeriousness? seriousness = null,
        Guid? addressId = null,
        string? searchTitle = null,
        Guid? companyId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Include(w => w.Address).AsQueryable();

        if(category.HasValue && category.Value != RequestCategory.None)
        {
            query = query.Where(w => w.Category == category.Value);
        }

        if(seriousness.HasValue && seriousness.Value != WorkSeriousness.Unknown)
        {
            query = query.Where(w => w.Seriousness == seriousness.Value);
        }

        if(addressId is not null)
        {
            query = query.Where(w => w.AddressId == addressId);
        }

        if(!string.IsNullOrEmpty(searchTitle))
        {
            query = query.Where(w => w.Title.Contains(searchTitle));
        }

        if(companyId is not null)
        {
            query = query.Where(w => w.Address.ServicedByCompanyId == companyId);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderByDescending(i => i.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<ICollection<Work>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(w => w.Address)
                .ThenInclude(a => a.FavoriteAddresses)
            .Where(w => w.Address.FavoriteAddresses.Any(fa => fa.UserId == userId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
