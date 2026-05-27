using MediatR;

namespace Hooome.Application.CQRS.Notifications.Commands.CreateSystemNotification;

public class CreateSystemNotificationCommand : IRequest<Guid>
{
    public string Text { get; set; } = null!;
}
