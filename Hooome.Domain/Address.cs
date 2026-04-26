namespace Hooome.Domain;

public class Address
{
    public Guid Id { get; set; }
    public string Street { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;

    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    //кем обслуживается
    public Guid? ServicedByCompanyId { get; set; }
    public Company? ServicedByCompany { get; set; }

    //какая компания тут расположена
    public Guid? RegisteredCompanyId { get; set; }
    public Company? RegisteredCompany { get; set; }
    
    public ICollection<FavoriteAddress> FavoriteAddresses { get; set; } = [];
    public ICollection<Work> Works { get; set; } = [];
    public ICollection<Request> Requests { get; set; } = [];
}
