using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestList;

public class GetRequestListQuery : IRequest<RequestListVm>
{
    public Guid UserId { get; set; }

    public RequestStatus? RequestStatus { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
