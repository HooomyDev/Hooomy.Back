using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatDetailsQuery;

public class GetChatDetailsQueryHandler(IChatRepository chatRepo, IMapper mapper)
    : IRequestHandler<GetChatDetailsQuery, ChatDetailsVm>
{
    public async Task<ChatDetailsVm> Handle(GetChatDetailsQuery request, CancellationToken cancellationToken)
    {
        var chat = await chatRepo.GetById(request.ChatId, cancellationToken)
            ?? throw new NotFoundException(nameof(Chat), request.ChatId);

        var chatVm = mapper.Map<ChatDetailsVm>(chat);

        return chatVm;
    }
}
