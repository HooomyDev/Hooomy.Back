using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Complaint
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ShortDescription { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ComplaintType Type { get; set; } = ComplaintType.Unknown;
    public ComplaintStatus Status { get; set; } = ComplaintStatus.Unknown;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public Guid? RequestId { get; set; }
    public Request? Request { get; set; }

    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }
}
