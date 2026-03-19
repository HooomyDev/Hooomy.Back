using AutoMapper;
using Hooome.Application.Chats.Commands.CreateChat;
using Hooome.Application.Chats.Queries.GetChatList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Authorize]
[Route("api/chats")]
public class ChatController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<ChatListVm>> GetAll()
    {
        var query = new GetChatListQuery
        {
            UserId = UserId
        };

        var chats = await Mediator.Send(query);

        return Ok(chats);
    }

    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create(CreateChatDto dto)
    {
        var command = mapper.Map<CreateChatCommand>(dto);
        command.ResidentId = UserId;

        var chatId = await Mediator.Send(command);

        return Ok(chatId);
    }
}
