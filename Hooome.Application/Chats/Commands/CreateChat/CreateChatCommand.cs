using MediatR;

namespace Hooome.Application.Chats.Commands.CreateChat;

public class CreateChatCommand : IRequest<Guid>
{
    public Guid ResidentId { get; set; }
    public string ResidentName { get; set; } = null!;
    public Guid CompanyId { get; set; }
}
