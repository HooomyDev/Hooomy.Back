namespace Hooome.Application.CQRS.Requests.Queries.GetMapData;

public class MapDataVm
{
    public List<GridCluster> Clusters { get; set; } = new();
    public int ZoomLevel { get; set; }
    public int TotalRequests { get; set; }
    public int TotalAddresses { get; set; }
}
