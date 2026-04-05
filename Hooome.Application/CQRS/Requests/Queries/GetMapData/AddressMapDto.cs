namespace Hooome.Application.CQRS.Requests.Queries.GetMapData;

public class AddressMapDto
{
    public Guid Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int RequestsCount { get; set; }
}
