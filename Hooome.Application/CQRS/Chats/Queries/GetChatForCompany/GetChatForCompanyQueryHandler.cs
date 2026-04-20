using AutoMapper;
using Hooome.Application.CQRS.Chats.Queries.GetChatList;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatForCompany;

public class GetChatForCompanyQueryHandler(IChatRepository chatRepo, IMapper mapper)
    : IRequestHandler<GetChatForCompanyQuery, ChatListForCompanyVm>
{
    public async Task<ChatListForCompanyVm> Handle(GetChatForCompanyQuery request, CancellationToken cancellationToken)
    {
        var chats = await chatRepo.GetAllByCompanyId(request.CompanyId, cancellationToken);

        var chatDtos = mapper.Map<List<ChatListLookupDto>>(chats);

        return new ChatListForCompanyVm { Chats =  chatDtos };
    }
}
