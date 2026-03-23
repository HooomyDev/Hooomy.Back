using Hooome.Application.Chats.Queries.GetChatList;

namespace Hooome.Application.Chats.Queries.GetChatForCompany;

public class ChatListForCompanyVm
{
    public IList<ChatListLookupDto> Chats { get; set; } = null!;
}
