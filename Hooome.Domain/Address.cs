namespace Hooome.Domain;

public class Address
{
    public Guid Id { get; set; }
    public string Street { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;

    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public ICollection<FavoriteAddress> FavoriteAddresses { get; set; } = [];
    public ICollection<Work> Works { get; set; } = [];
    public ICollection<Request> Requests { get; set; } = [];
}
