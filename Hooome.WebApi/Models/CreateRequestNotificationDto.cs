using AutoMapper;
using Hooome.Application.CQRS.Notifications.Commands.CreateRequestNotification;
using Hooome.Application.Common.Mappings;

namespace Hooome.WebApi.Models;

public class CreateRequestNotificationDto : IMapWith<CreateRequestNotificationCommand>
{
    public Guid RequestId { get; set; }
    public string Text { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateRequestNotificationDto, CreateRequestNotificationCommand>();
}
