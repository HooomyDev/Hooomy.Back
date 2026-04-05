using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Work
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid AddressId { get; set; }
    public RequestCategory Category { get; set; }
    public WorkSeriousness Seriousness { get; set; }
    public DateTime PlannedStartTime { get; set; }
    public DateTime PlannedEndTime { get; set; }
    public DateTime? FactStartTime { get; set; }
    public DateTime? FactEndTime { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Address Address { get; set; } = null!;
}
