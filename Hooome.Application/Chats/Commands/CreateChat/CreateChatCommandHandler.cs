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

        await dbContext.Chats.AddAsync(newChat, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newChat.Id;
    }
}
