using Hooome.Application.Interfaces;
using Hooome.Domain;

namespace Hooome.Persistance.Repositories;

public class SystemNotificationRepository(HooomeDbContext dbContext)
    : BaseRepository<SystemNotification>(dbContext), ISystemNotificationRepository
{
}
