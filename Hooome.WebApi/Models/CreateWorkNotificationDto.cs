using AutoMapper;
using Hooome.Application.CQRS.Notifications.Commands.CreateWorkNotification;
using Hooome.Application.Common.Mappings;

namespace Hooome.WebApi.Models;

public class CreateWorkNotificationDto : IMapWith<CreateWorkNotificationCommand>
{
    public Guid WorkId { get; set; }
    public string Text { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateWorkNotificationDto, CreateWorkNotificationCommand>();
}
