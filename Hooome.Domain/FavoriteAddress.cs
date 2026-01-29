namespace Hooome.Domain;

public class FavoriteAddress
{
    public Guid Id { get; set; }
    public string Street { get; set; } = null!;
    public int House { get; set; }
    public string Pseudonym { get; set; } = null!;
    public Guid UserID { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
