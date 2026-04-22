using AutoMapper;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatList;

public class GetChatListQueryHandler(IChatRepository chatRepo, IMapper mapper)
    : IRequestHandler<GetChatListQuery, ChatListVm>
{
    public async Task<ChatListVm> Handle(GetChatListQuery request, CancellationToken cancellationToken)
    {
        var chats = await chatRepo.GetAllByResidentId(request.UserId, cancellationToken);

        var chatDtos = mapper.Map<List<ChatListLookupDto>>(chats);

        return new ChatListVm { Chats = chatDtos };
    }
}
