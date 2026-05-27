using AutoMapper;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Notifications.Queries.GetNotifications;

public class GetNotificationsQueryHandler(
    ISystemNotificationRepository systemNotificationRepo,
    IWorkNotificationRepository workNotificationRepo,
    IRequestNotificationRepository requestNotificationRepo,
    IMapper mapper)
    : IRequestHandler<GetNotificationsQuery, NotificationListVm>
{
    public async Task<NotificationListVm> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        // fetch all notifications from each repository 
        var systemNotifications = await systemNotificationRepo.GetAll(cancellationToken);

        var requestNotifications = await requestNotificationRepo.GetByUserIdAsync(
            request.UserId, cancellationToken);

        var workNotifications = await workNotificationRepo.GetByUserIdAsync(
            request.UserId, cancellationToken);

        // map to DTOs
        var notifications = new List<NotificationDto>();
        notifications.AddRange(systemNotifications.Select(sn => mapper.Map<NotificationDto>(sn)));
        notifications.AddRange(requestNotifications.Select(rn => mapper.Map<NotificationDto>(rn)));
        notifications.AddRange(workNotifications.Select(wn => mapper.Map<NotificationDto>(wn)));

        var totalCount = systemNotifications.Count() + requestNotifications.Count() + systemNotifications.Count();

        // order by date and apply pagination
        var paginated = notifications
            .OrderByDescending(n => n.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new NotificationListVm { 
            Notifications = paginated,
            TotalCount = totalCount,
            PageSize = request.PageSize,
            Page = request.Page,
        };
    }
}
