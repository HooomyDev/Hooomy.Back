using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Commands.UpdateRequest;

public class UpdateRequestCommandValidator
    : AbstractValidator<UpdateRequestCommand>
{
    public UpdateRequestCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty().MaximumLength(150);
        RuleFor(c => c.AddressId).NotEmpty();
        RuleFor(c => c.Description).NotEmpty().MaximumLength(300);
        RuleFor(c => c.Category).NotEmpty();
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
        RuleFor(c => c.Id).NotEqual(Guid.Empty);
    }
}
