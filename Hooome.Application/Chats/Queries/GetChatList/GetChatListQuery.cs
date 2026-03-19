using MediatR;

namespace Hooome.Application.Chats.Queries.GetChatList;

public class GetChatListQuery : IRequest<ChatListVm>
{
    public Guid UserId { get; set; }
}
