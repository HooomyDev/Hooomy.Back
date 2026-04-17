using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Request
{
    public Guid UserID { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid AddressId { get; set; }
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public RequestCategory Category { get; set; } = RequestCategory.Other;
    public string PhotoUrl { get; set; } = string.Empty;
    public DateTime CreatedAt {  get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public Address Address { get; set; } = null!;
    public ICollection<RequestImage> Images { get; set; } = [];
}
