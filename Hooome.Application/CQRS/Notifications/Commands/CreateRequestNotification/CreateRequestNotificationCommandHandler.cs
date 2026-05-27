using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Notifications.Commands.CreateRequestNotification;

public class CreateRequestNotificationCommandHandler(IRequestNotificationRepository requestNotificationRepo)
    : IRequestHandler<CreateRequestNotificationCommand, Guid>
{
    public async Task<Guid> Handle(CreateRequestNotificationCommand request, CancellationToken cancellationToken)
    {
        var requestNotification = new RequestNotification
        {
            Id = Guid.NewGuid(),
            RequestId = request.RequestId,
            Text = request.Text,
            CreatedAt = DateTime.UtcNow,
        };

        await requestNotificationRepo.Create(requestNotification, cancellationToken);

        return requestNotification.Id;
    }
}
