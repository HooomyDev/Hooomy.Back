using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Commands.UpdateRequest;

public class UpdateRequestCommandValidator
    : AbstractValidator<UpdateRequestCommand>
{
    public UpdateRequestCommandValidator()
    {
        RuleFor(c => c.Title).MaximumLength(150);
        RuleFor(c => c.Description).MaximumLength(300);
        RuleFor(c => c.Id).NotEqual(Guid.Empty);
    }
}
