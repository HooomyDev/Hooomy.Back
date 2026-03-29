using Hooome.Application.CQRS.Messages.Queries.GetMessageList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[ApiController]
[Route("api/chats/{chatId:guid}/messages")]
[Authorize]
public class MessageController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<MessageListVm>> Get(Guid chatId)
    {
        var query = new GetMessageListQuery()
        {
            ChatId = chatId
        };

        var messages = await Mediator.Send(query);

        return Ok(messages);
    }
}
