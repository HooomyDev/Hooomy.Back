using FluentValidation;

namespace Hooome.Application.CQRS.Polls.Commands.UpdatePoll;

public class UpdatePollCommandValidator
    : AbstractValidator<UpdatePollCommand>
{
    public UpdatePollCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);
        
        RuleFor(p => p.CreatedBy)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(p => p.IsActive)
            .NotEmpty();
    }
}