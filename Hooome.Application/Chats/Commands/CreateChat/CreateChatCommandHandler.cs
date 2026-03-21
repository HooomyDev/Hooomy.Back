using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.Chats.Commands.CreateChat;

public class CreateChatCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<CreateChatCommand, Guid>
{
    public async Task<Guid> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var newChat = new Chat
        {
            Id = Guid.NewGuid(),
            CompanyId = request.CompanyId,
            ResidentId = request.ResidentId,
            Status = Domain.Enums.ChatStatus.Active,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
        };

        var isChatExist = dbContext.Chats
            .Any(x => x.CompanyId == newChat.CompanyId && x.ResidentId == request.ResidentId);

        if(isChatExist)
        {
            throw new AlreadyExistException("Chat already exists");
        }

        await dbContext.Chats.AddAsync(newChat, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newChat.Id;
    }
}
