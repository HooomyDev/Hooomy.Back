using Hooome.Application.Messages.Commands.CreateMessage;

namespace Hooome.Application.Messages.Queries.GetMessageList;

public class MessageListVm
{
    public IList<MessageDetailsVm> Messages { get; set; } = [];
}
