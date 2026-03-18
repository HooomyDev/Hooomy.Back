using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Message
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public SenderType SenderType { get; set; }
    public string SenderName { get; set; } = null!;
    public Guid SenderId { get; set; }
    public string Content { get; set; } = null!;
    public MessageType MessageType { get; set; }
    public bool IsRead { get; set; }

    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Chat Chat { get; set; } = null!;
}
