using FluentValidation;

namespace Hooome.Application.CQRS.Notifications.Commands.CreateWorkNotification;

public class CreateWorkNotificationCommandValidator
    : AbstractValidator<CreateWorkNotificationCommand>
{
    public CreateWorkNotificationCommandValidator()
    {
        RuleFor(x => x.WorkId).NotEmpty();
        RuleFor(x => x.Text).NotEmpty().NotNull().MaximumLength(500);
    }
}
