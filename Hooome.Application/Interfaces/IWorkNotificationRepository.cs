using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IWorkNotificationRepository : IRepository<WorkNotification>
{
    Task<IEnumerable<WorkNotification>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
