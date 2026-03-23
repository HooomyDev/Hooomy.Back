using MediatR;

namespace Hooome.Application.Chats.Queries.GetChatForCompany;

public class GetChatForCompanyQuery : IRequest<ChatListForCompanyVm>
{
    public Guid CompanyId { get; set; }
}
