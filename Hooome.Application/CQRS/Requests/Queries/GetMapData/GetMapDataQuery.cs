using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetMapData;

public class GetMapDataQuery : IRequest<MapDataVm>
{
    public RequestStatus RequestStatus { get; set; }
    public int Month { get; set; }
    public int ZoomLevel { get; set; }
}
