using FluentValidation;

namespace Hooome.Application.CQRS.Notifications.Queries.GetNotifications;

public class GetNotificationsQueryValidator
    : AbstractValidator<GetNotificationsQuery>
{
    public GetNotificationsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotEqual(Guid.Empty);
    }
}
