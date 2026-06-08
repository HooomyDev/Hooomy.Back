using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Complaint
{
    public Guid Id { get; set; }
    public string ShortDescription { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ComplaintType Type { get; set; } = ComplaintType.Unknown;
    public ComplaintStatus Status { get; set; } = ComplaintStatus.Unknown;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}