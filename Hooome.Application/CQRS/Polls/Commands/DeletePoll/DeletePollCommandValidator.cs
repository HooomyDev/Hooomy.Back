using FluentValidation;

namespace Hooome.Application.CQRS.Polls.Commands.DeletePoll;

public class DeletePollCommandValidator
    : AbstractValidator<DeletePollCommand>
{
    public DeletePollCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(p => p.CreatedBy)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}
