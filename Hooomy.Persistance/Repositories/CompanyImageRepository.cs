using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class CompanyImageRepository(HooomeDbContext dbContext)
    : BaseRepository<CompanyImage>(dbContext), ICompanyImageRepository
{
    public async Task<CompanyImage?> GetLogoByCompanyId(Guid companyId,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(i => i.CompanyId == companyId, cancellationToken);
    }
}
