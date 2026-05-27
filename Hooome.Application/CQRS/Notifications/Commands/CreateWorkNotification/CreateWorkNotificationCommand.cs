using MediatR;

namespace Hooome.Application.CQRS.Notifications.Commands.CreateWorkNotification;

public class CreateWorkNotificationCommand : IRequest<Guid>
{
    public Guid WorkId { get; set; }
    public string Text { get; set; } = null!;
}
