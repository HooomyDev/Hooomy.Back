using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Request
{
    public Guid UserID { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public RequestCategory Category { get; set; } = RequestCategory.Other;
    public string PhotoUrl { get; set; } = string.Empty;
    public DateTime CreatedAt {  get; set; }
    public DateTime? UpdatedAt { get; set; }
}
