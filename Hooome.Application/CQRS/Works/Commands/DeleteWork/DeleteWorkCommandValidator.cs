using FluentValidation;

namespace Hooome.Application.CQRS.Works.Commands.DeleteWork;

public class DeleteWorkCommandValidator
    : AbstractValidator<DeleteWorkCommand>
{
    public DeleteWorkCommandValidator()
    {
        RuleFor(w => w.WorkId).NotEmpty().NotEqual(Guid.Empty);
    }
}