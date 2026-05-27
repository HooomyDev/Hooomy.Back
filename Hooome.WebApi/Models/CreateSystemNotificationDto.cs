using AutoMapper;
using Hooome.Application.CQRS.Notifications.Commands.CreateSystemNotification;
using Hooome.Application.Common.Mappings;

namespace Hooome.WebApi.Models;

public class CreateSystemNotificationDto : IMapWith<CreateSystemNotificationCommand>
{
    public string Text { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateSystemNotificationDto, CreateSystemNotificationCommand>();
}
