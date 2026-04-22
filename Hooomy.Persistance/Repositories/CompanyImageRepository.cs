using Hooome.Application.Interfaces;
using Hooome.Domain;

namespace Hooome.Persistance.Repositories;

public class CompanyImageRepository(HooomeDbContext dbContext)
    : BaseRepository<CompanyImage>(dbContext), ICompanyImageRepository
{
    
}
