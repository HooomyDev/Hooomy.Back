using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Notifications.Commands.CreateWorkNotification;

public class CreateWorkNotificationCommandHandler(IWorkNotificationRepository workNotificationRepo)
    : IRequestHandler<CreateWorkNotificationCommand, Guid>
{
    public async Task<Guid> Handle(CreateWorkNotificationCommand request, CancellationToken cancellationToken)
    {
        var workNotification = new WorkNotification
        {
            Id = Guid.NewGuid(),
            WorkId = request.WorkId,
            Text = request.Text,
            CreatedAt = DateTime.UtcNow,
        };

        await workNotificationRepo.Create(workNotification, cancellationToken);

        return workNotification.Id;
    }
}
