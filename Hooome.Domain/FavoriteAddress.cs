namespace Hooome.Domain;

public class FavoriteAddress
{
    public Guid Id { get; set; }
    public Guid AddressId { get; set; }
    public string Pseudonym { get; set; } = null!;
    public Guid UserID { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Address Address { get; set; } = null!;
}
