using AutoMapper;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Messages.Commands.CreateMessage;

public class CreateMessageCommandHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<CreateMessageCommand, MessageDetailsVm>
{
    public async Task<MessageDetailsVm> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var newMessage = new Message()
        {
            Id = Guid.NewGuid(),
            ChatId = request.ChatId,
            SenderId = request.SenderId,
            SenderType = request.SenderType,
            SenderName = request.SenderName,
            Content = request.Content,
            MessageType = request.MessageType,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            ReadAt = null
        };

        await dbContext.Messages.AddAsync(newMessage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<MessageDetailsVm>(newMessage);
    }
}
