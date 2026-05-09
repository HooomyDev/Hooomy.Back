using FluentValidation;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Polls.Commands.CreatePoll;

public class CreatePollCommandValidator 
    : AbstractValidator<CreatePollCommand>
{
    public CreatePollCommandValidator()
    {
        RuleFor(p => p.CreatedBy)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(p => p.CompanyId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(p => p.Type)
            .IsInEnum()
            .NotEqual(PollType.Unknown);

        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(p => p.Options)
            .NotNull()
            .Must(o => o.Count > 0)
            .Must(o => o.Count <= 5);

        RuleForEach(p => p.Options)
            .SetValidator(new PollOptionDtoValidator());
    }
}
