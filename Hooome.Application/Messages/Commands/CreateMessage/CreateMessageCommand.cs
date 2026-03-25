using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.Messages.Commands.CreateMessage;

public class CreateMessageCommand : IRequest<MessageDetailsVm>
{
    public Guid ChatId { get; set; }
    public SenderType SenderType { get; set; }
    public string SenderName { get; set; } = null!;
    public Guid SenderId { get; set; }
    public string Content { get; set; } = null!;
    public MessageType MessageType { get; set; }
}
