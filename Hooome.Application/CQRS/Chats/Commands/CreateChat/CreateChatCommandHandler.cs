using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Chats.Commands.CreateChat;

public class CreateChatCommandHandler(IChatRepository chatRepo)
    : IRequestHandler<CreateChatCommand, Guid>
{
    public async Task<Guid> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var newChat = new Chat
        {
            Id = Guid.NewGuid(),
            CompanyId = request.CompanyId,
            ResidentId = request.ResidentId,
            ResidentName = request.ResidentName,
            Status = Domain.Enums.ChatStatus.Active,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
        };

        var isChatExist = await chatRepo
            .IsChatExist(request.CompanyId, request.ResidentId, cancellationToken);

        if(isChatExist)
        {
            throw new AlreadyExistException("Chat already exists");
        }

        await chatRepo.Create(newChat, cancellationToken);

        return newChat.Id;
    }
}
