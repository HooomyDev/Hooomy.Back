using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Request
{
    public Guid UserID { get; set; }
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public RequestCategory Category { get; set; } = RequestCategory.Other;
}
