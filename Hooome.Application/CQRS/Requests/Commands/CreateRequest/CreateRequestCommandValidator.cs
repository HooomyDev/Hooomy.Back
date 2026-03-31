using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Commands.CreateRequest;

public class CreateRequestCommandValidator 
    : AbstractValidator<CreateRequestCommand>
{
    public CreateRequestCommandValidator()
    {
        RuleFor(c => c.Address).NotEmpty().MaximumLength(250);
        RuleFor(c => c.Title).NotEmpty().MaximumLength(150);
        RuleFor(c => c.Description).NotEmpty().MaximumLength(300);
        RuleFor(c => c.Category).NotEmpty();
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
    }
}
