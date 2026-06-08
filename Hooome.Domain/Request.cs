using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Request
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public RequestCategory Category { get; set; } = RequestCategory.Other;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public Guid? ReviewId { get; set; } 
    public RequestReview? Review { get; set; }

    public Guid AddressId { get; set; }
    public Address Address { get; set; } = null!;

    public ICollection<RequestImage> Images { get; set; } = [];
    public ICollection<RequestComment> Comments { get; set; } = [];
    public ICollection<RequestNotification> Notifications { get; set; } = [];
}