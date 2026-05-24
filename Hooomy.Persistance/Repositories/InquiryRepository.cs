using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class InquiryRepository(HooomeDbContext dbContext)
    : BaseRepository<Inquiry>(dbContext), IInquiryRepository
{
    public async Task<(ICollection<Inquiry> Items, int TotalCount)> GetAllWithPagination(
        int page = 1,
        int pageSize = 10,
        DateTime? date = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (date.HasValue)
        {
            var startDate = date.Value.Date;
            var endDate = startDate.AddDays(1);
            query = query.Where(i => i.CreatedAt.Date >= startDate && i.CreatedAt.Date < endDate);
        }

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);
        
        var totalCount = await query.CountAsync(cancellationToken);

        return (items, totalCount);
    }
}
