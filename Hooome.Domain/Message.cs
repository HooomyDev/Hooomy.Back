using Hooome.Domain.Enums;

namespace Hooome.Domain;

public class Message
{
    public Guid Id { get; set; }
    public SenderType SenderType { get; set; } = SenderType.Unknown;
    public string SenderName { get; set; } = null!;
    public Guid SenderId { get; set; }
    public string Content { get; set; } = null!;
    public MessageType MessageType { get; set; } = MessageType.Unknown;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid ChatId { get; set; }
    public Chat Chat { get; set; } = null!;
}