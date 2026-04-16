using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Commands.SoftDeleteRequest;

public class SoftDeleteRequestCommandValidator
    : AbstractValidator<SoftDeleteRequestCommand>
{
    public SoftDeleteRequestCommandValidator()
    {
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
        RuleFor(c => c.Id).NotEqual(Guid.Empty);
    }
}