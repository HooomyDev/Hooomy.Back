using Hooome.Application.CQRS.Messages.Commands.CreateMessage;

namespace Hooome.Application.CQRS.Messages.Queries.GetMessageList;

public class MessageListVm
{
    public IList<MessageDetailsVm> Messages { get; set; } = [];
}
