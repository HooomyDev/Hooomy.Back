using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatDetailsQuery;

public class ChatDetailsVm : IMapWith<Chat>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; } = null!;
    public Guid ResidentId { get; set; }    
    public string ResidentName { get; set; } = null!;
    public ChatStatus Status { get; set; } = ChatStatus.Unknown;
    public ICollection<MessageVm> Messages { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastMessageSentAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Chat, ChatDetailsVm>()
            .ForMember(dest => dest.CompanyId,
                opt => opt.MapFrom(src => src.Company.Id))
            .ForMember(dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.LastMessageSentAt,
                opt => opt.MapFrom(src => src.Messages != null && src.Messages.Any()
                    ? src.Messages.Max(m => m.CreatedAt)
                    : (DateTime?)null));
    }
}
