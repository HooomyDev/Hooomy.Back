using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Notifications.Commands.CreateSystemNotification;

public class CreateSystemNotificationCommandHandler(ISystemNotificationRepository systemNotificationRepo)
    : IRequestHandler<CreateSystemNotificationCommand, Guid>
{
    public async Task<Guid> Handle(CreateSystemNotificationCommand request, CancellationToken cancellationToken)
    {
        var systemNotification = new SystemNotification
        {
            Id = Guid.NewGuid(),
            Text = request.Text,
            CreatedAt = DateTime.UtcNow,
        };

        await systemNotificationRepo.Create(systemNotification, cancellationToken);

        return systemNotification.Id;
    }
}