using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class ComplaintRepository(HooomeDbContext dbContext)
    : BaseRepository<Complaint>(dbContext), IComplaintRepository
{
    public async Task<List<Complaint>> GetAllWithFilters(ComplaintStatus status, 
        ComplaintType type, 
        string? shortDescription, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (status != ComplaintStatus.Unknown)
        {
            query = query.Where(c => c.Status == status);
        }

        if (type != ComplaintType.Unknown)
        {
            query = query.Where(c => c.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(shortDescription))
        {
            query = query.Where(c => c.ShortDescription.Contains(shortDescription));
        }

        query = query.OrderByDescending(c => c.CreatedAt);

        return await query.ToListAsync(cancellationToken);
    }
}
