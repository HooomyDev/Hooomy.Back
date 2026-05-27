using MediatR;

namespace Hooome.Application.CQRS.Notifications.Commands.CreateRequestNotification;

public class CreateRequestNotificationCommand : IRequest<Guid>
{
    public Guid RequestId { get; set; }
    public string Text { get; set; } = null!;
}
