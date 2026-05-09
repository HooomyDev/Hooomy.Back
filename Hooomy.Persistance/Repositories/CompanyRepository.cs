using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class CompanyRepository(HooomeDbContext dbContext)
    : BaseRepository<Company>(dbContext), ICompanyRepository
{
    public override async Task<Company?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.ServedAddresses)
            .Include(c => c.Address)
            .Include(c => c.Logo)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Company>> GetAll(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.ServedAddresses)
            .Include(c => c.Address)
            .Include(c => c.Logo)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
