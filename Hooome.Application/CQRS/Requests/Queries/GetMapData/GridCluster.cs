namespace Hooome.Application.CQRS.Requests.Queries.GetMapData;

public class GridCluster
{
    public Guid Id { get; set; }
    public int CellX { get; set; }
    public int CellY { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int AddressesCount { get; set; }
    public int TotalRequests { get; set; }
    public List<AddressMapDto> Addresses { get; set; } = new();
    public (double MinLat, double MaxLat, double MinLng, double MaxLng) Bounds { get; set; }
}