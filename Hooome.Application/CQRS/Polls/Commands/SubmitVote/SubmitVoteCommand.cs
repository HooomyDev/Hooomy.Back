using Hooome.Application.CQRS.Polls.Queries.GetPollDetails;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.SubmitVote;

public class SubmitVoteCommand : IRequest
{
    public Guid PollId { get; set; }
    public Guid UserId { get; set; }
    public SubmitVoteDto Vote { get; set; } = null!;
}
