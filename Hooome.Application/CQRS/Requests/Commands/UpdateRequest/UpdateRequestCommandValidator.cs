using FluentValidation;

namespace Hooome.Application.Requests.Commands.UpdateRequest;

public class UpdateRequestCommandValidator
    : AbstractValidator<UpdateRequestCommand>
{
    public UpdateRequestCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty().MaximumLength(150);
        RuleFor(c => c.Address).NotEmpty().MaximumLength(250);
        RuleFor(c => c.Description).NotEmpty().MaximumLength(300);
        RuleFor(c => c.Category).NotEmpty();
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
        RuleFor(c => c.Id).NotEqual(Guid.Empty);
        RuleFor(c => c.PhotoUrl).MaximumLength(250);
    }
}
