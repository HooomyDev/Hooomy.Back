namespace Hooome.Domain;

public class FavoriteAddress
{
    public Guid Id { get; set; }
    public string Pseudonym { get; set; } = null!;
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Guid AddressId { get; set; }
    public Address Address { get; set; } = null!;
}