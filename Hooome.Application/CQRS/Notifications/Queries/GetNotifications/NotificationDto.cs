using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Notifications.Queries.GetNotifications;

public class NotificationDto : 
    IMapWith<SystemNotification>,
    IMapWith<WorkNotification>,
    IMapWith<RequestNotification>
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public NotificationType Type { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SystemNotification, NotificationDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => NotificationType.System));

        profile.CreateMap<WorkNotification, NotificationDto>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => NotificationType.Work))
            .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.WorkId));

        profile.CreateMap<RequestNotification, NotificationDto>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => NotificationType.Request))
            .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.RequestId));
    }
}
