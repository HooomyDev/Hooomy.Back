using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Chat
{
    public Guid Id { get; set; }
    public Guid ResidentId { get; set; }
    public Guid CompanyId { get; set; }
    public ChatStatus Status { get; set; } = ChatStatus.Unknown;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Company Company { get; set; } = null!;
    public ICollection<Message> Messages { get; set; } = [];
}
