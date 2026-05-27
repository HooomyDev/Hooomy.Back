using FluentValidation;

namespace Hooome.Application.CQRS.Notifications.Commands.CreateRequestNotification;

public class CreateRequestNotificationCommandValidator
    : AbstractValidator<CreateRequestNotificationCommand>
{
    public CreateRequestNotificationCommandValidator()
    {
        RuleFor(x => x.RequestId).NotEmpty();
        RuleFor(x => x.Text).NotEmpty().NotNull().MaximumLength(500);
    }
}
