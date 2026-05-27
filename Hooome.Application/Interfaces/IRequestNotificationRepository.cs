using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IRequestNotificationRepository : IRepository<RequestNotification>
{
    Task<IEnumerable<RequestNotification>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}