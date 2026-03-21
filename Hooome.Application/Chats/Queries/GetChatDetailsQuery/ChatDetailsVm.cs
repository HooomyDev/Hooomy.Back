using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.Chats.Queries.GetChatDetailsQuery;

public class ChatDetailsVm
{
    public Guid Id { get; set; }
    public Guid ResidentId { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; } = null!;
    public ChatStatus Status { get; set; } = ChatStatus.Unknown;
    public ICollection<Message> Messages { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastMessageSentAt { get; set; }
}
