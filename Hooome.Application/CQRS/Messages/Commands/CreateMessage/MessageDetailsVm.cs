
using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Messages.Commands.CreateMessage;

public class MessageDetailsVm : IMapWith<Message>
{
    public Guid Id { get; set; }
    public SenderType SenderType { get; set; }
    public string SenderName { get; set; } = null!;
    public string Content { get; set; } = null!;
    public MessageType MessageType { get; set; }
    public bool IsRead { get; set; }

    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void Mapping(Profile profile)
        => profile.CreateMap<Message, MessageDetailsVm>();
}
