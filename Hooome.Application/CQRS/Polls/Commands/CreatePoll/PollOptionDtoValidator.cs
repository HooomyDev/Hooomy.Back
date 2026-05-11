using FluentValidation;

namespace Hooome.Application.CQRS.Polls.Commands.CreatePoll;

public class PollOptionDtoValidator : AbstractValidator<PollOptionDto>
{
    public PollOptionDtoValidator()
    {
        RuleFor(o => o.Content)
            .NotEmpty()
            .MaximumLength(500);
    }
}