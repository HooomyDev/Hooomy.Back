using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDetails;

public class GetRequestDetailsQuery : IRequest<RequestDetailsVm>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}
