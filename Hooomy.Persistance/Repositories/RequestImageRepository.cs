using Hooome.Application.Interfaces;
using Hooome.Domain;

namespace Hooome.Persistance.Repositories;

public class RequestImageRepository(HooomeDbContext dbContext)
    : BaseRepository<RequestImage>(dbContext), IRequestImageRepository
{

}
