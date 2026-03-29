using MediatR;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatDetailsQuery;

public class GetChatDetailsQuery : IRequest<ChatDetailsVm>
{
    public Guid ResidentId { get; set; }
    public Guid ChatId { get; set; }
}
