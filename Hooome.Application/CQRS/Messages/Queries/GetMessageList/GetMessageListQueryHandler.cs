using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hooome.Application.CQRS.Messages.Commands.CreateMessage;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Messages.Queries.GetMessageList;

public class GetMessageListQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetMessageListQuery, MessageListVm>
{
    public async Task<MessageListVm> Handle(GetMessageListQuery request, CancellationToken cancellationToken)
    {
        var messages = await dbContext.Messages
            .Where(x => x.ChatId == request.ChatId)
            .ProjectTo<MessageDetailsVm>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new MessageListVm { Messages = messages };
    }
}
