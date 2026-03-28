using FluentValidation;

namespace Hooome.Application.CQRS.Polls.Commands.SubmitVote;

public class SubmitVoteCommandValidator
    : AbstractValidator<SubmitVoteCommand>
{
    public SubmitVoteCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(x => x.PollId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(x => x.Vote).NotEmpty();
    }
}