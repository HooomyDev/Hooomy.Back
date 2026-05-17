using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class RequestComment
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public string Text { get; set; } = null!;
    public RequestCommentStatus Status { get; set; } = RequestCommentStatus.Unknown;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    public Company Company { get; set; } = null!;
    public Request Request { get; set; } = null!;
    public ICollection<RequestCommentImage> Images { get; set; } = [];
}
