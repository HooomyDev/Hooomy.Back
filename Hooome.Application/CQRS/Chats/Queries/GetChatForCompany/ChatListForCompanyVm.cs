using Hooome.Application.CQRS.Chats.Queries.GetChatList;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatForCompany;

public class ChatListForCompanyVm
{
    public IList<ChatListLookupDto> Chats { get; set; } = null!;
}
