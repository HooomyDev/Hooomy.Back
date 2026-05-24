using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCount;

public class GetRequestCountQuery : IRequest<int>
{
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
}
