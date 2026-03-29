using MediatR;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatList;

public class GetChatListQuery : IRequest<ChatListVm>
{
    public Guid UserId { get; set; }
}
