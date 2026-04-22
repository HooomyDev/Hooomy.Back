using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Messages.Commands.CreateMessage;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class CreateMessageDto : IMapWith<CreateMessageCommand>
{
    public Guid ChatId { get; set; }
    public SenderType SenderType { get; set; }
    public string SenderName { get; set; } = null!;
    public MessageType MessageType { get; set; }
    public string Content { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateMessageDto, CreateMessageCommand>();
}
