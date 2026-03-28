using MediatR;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollDetails;

public class GetPollDetailsQuery : IRequest<PollDetailsVm>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}
