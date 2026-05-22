using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Request
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid AddressId { get; set; }
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public RequestCategory Category { get; set; } = RequestCategory.Other;
    public DateTime CreatedAt {  get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public Address Address { get; set; } = null!;
    public RequestReview? Review { get; set; }
    public ICollection<RequestImage> Images { get; set; } = [];
    public ICollection<RequestComment> Comments { get; set; } = [];
}
