using MediatR;

namespace Hooome.Application.CQRS.Messages.Queries.GetMessageList;

public class GetMessageListQuery : IRequest<MessageListVm>
{
    public Guid ChatId { get; set; }
}
