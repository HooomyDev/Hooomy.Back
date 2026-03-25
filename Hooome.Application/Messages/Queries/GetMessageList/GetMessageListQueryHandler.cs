using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hooome.Application.Interfaces;
using Hooome.Application.Messages.Commands.CreateMessage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.Messages.Queries.GetMessageList;

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
