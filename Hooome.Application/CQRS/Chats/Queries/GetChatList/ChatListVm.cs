namespace Hooome.Application.CQRS.Chats.Queries.GetChatList;

public class ChatListVm
{
    public IList<ChatListLookupDto> Chats { get; set; } = [];
}
