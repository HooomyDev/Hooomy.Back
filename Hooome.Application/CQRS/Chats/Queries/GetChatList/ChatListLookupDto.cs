using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatList;

public class ChatListLookupDto : IMapWith<Chat>
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public string LastMessageContent { get; set; } = null!;
    public DateTime? LastMessageSentAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int UnreadCount { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Chat, ChatListLookupDto>();
}
