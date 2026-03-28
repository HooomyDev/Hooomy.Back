using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Polls.Commands.SubmitVote;

public class SubmitVoteDto
{
    public PollType Type { get; set; }

    public Guid? OptionId { get; set; }
    public List<Guid>? OptionIds { get; set; }
}
