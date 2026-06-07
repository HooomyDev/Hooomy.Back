using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class PollRepository(HooomeDbContext dbContext) 
    : BaseRepository<Poll>(dbContext), IPollRepository
{

    public override async Task<Poll?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Company)
            .Include(p => p.Options)
                .ThenInclude(po => po.Votes)
            .Include(p => p.Votes)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<(IEnumerable<Poll> Items, int TotalCount)> GetFilteredPolls(string? title = null,
        int page = 1,
        int pageSize = 10,
        PollType? type = PollType.Unknown,
        PollStatus? status = PollStatus.Unknown,
        Guid? companyId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(p => p.Company)
            .Include(p => p.Votes)
            .AsQueryable();

        if(companyId is not null)
        {
            query = query.Where(p => p.CompanyId == companyId);
        }

        if(!string.IsNullOrEmpty(title))
        {
            query = query.Where(p => EF.Functions.Like(p.Title, $"{title}%"));
        }

        if(type != PollType.Unknown)
        {
            query = query.Where(p => p.Type == type);
        }

        if(status != PollStatus.Unknown)
        {
            query = query.Where(p => p.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
