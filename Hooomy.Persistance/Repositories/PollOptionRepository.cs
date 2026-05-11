using Hooome.Application.Interfaces;
using Hooome.Domain;

namespace Hooome.Persistance.Repositories;

public class PollOptionRepository(HooomeDbContext dbContext)
    : BaseRepository<PollOption>(dbContext), IPollOptionRepository
{

}