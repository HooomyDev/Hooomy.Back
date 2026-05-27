namespace Hooome.Application.CQRS.Notifications.Queries.GetNotifications;

public class NotificationListVm
{
    public IList<NotificationDto> Notifications { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
}
