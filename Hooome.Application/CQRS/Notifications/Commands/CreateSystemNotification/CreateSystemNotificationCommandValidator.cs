using FluentValidation;

namespace Hooome.Application.CQRS.Notifications.Commands.CreateSystemNotification;

public class CreateSystemNotificationCommandValidator 
    : AbstractValidator<CreateSystemNotificationCommand>
{
    public CreateSystemNotificationCommandValidator()
    {
        RuleFor(x => x.Text).NotEmpty().NotNull().MaximumLength(500);
    }
}