namespace Hooome.Domain;

public class Address
{
    public Guid Id { get; set; }
    public string Street { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;

    public ICollection<FavoriteAddress> FavoriteAddresses { get; set; } = [];
    public ICollection<Work> Works { get; set; } = [];
}
