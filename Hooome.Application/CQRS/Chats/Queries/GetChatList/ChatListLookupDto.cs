using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatList;

public class ChatListLookupDto : IMapWith<Chat>
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = null!;
    public string ResidentName { get; set; } = null!;
    public string LastMessageContent { get; set; } = null!;
    public DateTime? LastMessageSentAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int UnreadCount { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Chat, ChatListLookupDto>()
            .ForMember(dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.LastMessageContent,
                opt => opt.MapFrom(src => src.Messages != null && src.Messages.Any()
                    ? src.Messages.OrderByDescending(m => m.CreatedAt).First().Content
                    : string.Empty))
            .ForMember(dest => dest.LastMessageSentAt,
                opt => opt.MapFrom(src => src.Messages != null && src.Messages.Any()
                    ? src.Messages.Max(m => m.CreatedAt)
                    : (DateTime?)null))
            .ForMember(dest => dest.UnreadCount,
                opt => opt.MapFrom(src => src.Messages != null
                    ? src.Messages.Count(m => !m.IsRead)
                    : 0));
    }
}
