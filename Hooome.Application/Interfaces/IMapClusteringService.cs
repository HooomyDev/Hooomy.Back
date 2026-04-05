using Hooome.Application.CQRS.Requests.Queries.GetMapData;

namespace Hooome.Application.Interfaces;

public interface IMapClusteringService
{
    List<GridCluster> ClusterAddresses(List<AddressMapDto> addresses, double? cellSize = null);
}
