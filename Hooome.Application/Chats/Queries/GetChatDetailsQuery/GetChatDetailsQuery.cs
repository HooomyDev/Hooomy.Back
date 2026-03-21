using MediatR;

namespace Hooome.Application.Chats.Queries.GetChatDetailsQuery;

public class GetChatDetailsQuery : IRequest<ChatDetailsVm>
{
    public Guid ResidentId { get; set; }
    public Guid CompanyId { get; set; }
}
