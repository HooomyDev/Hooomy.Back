using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestList;

public class GetRequestListQuery : IRequest<RequestListVm>
{
    public Guid UserId { get; set; }
}
