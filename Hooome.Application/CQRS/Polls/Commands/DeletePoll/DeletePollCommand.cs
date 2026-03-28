using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.DeletePoll;

public class DeletePollCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
}
