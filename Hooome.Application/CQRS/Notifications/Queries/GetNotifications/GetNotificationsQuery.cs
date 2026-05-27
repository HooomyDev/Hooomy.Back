using MediatR;

namespace Hooome.Application.CQRS.Notifications.Queries.GetNotifications;

public class GetNotificationsQuery : IRequest<NotificationListVm>
{
    public Guid UserId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 15;
}
