using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class RequestComment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Text { get; set; } = null!;
    public RequestCommentStatus Status { get; set; } = RequestCommentStatus.Unknown;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public Guid RequestId { get; set; }
    public Request Request { get; set; } = null!;
    
    public ICollection<RequestCommentImage> Images { get; set; } = [];
}