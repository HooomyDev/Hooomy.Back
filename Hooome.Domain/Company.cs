namespace Hooome.Domain;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string WorkingHours { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Poll> Polls { get; set; } = [];

    public Guid? LogoId { get; set; }
    public CompanyImage? Logo { get; set; }

    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }

    public ICollection<Address> ServedAddresses { get; set; } = [];
}
