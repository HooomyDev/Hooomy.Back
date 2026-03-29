using AutoMapper;
using Hooome.Application.CQRS.Chats.Commands.CreateChat;
using Hooome.Application.CQRS.Chats.Queries.GetChatDetailsQuery;
using Hooome.Application.CQRS.Chats.Queries.GetChatForCompany;
using Hooome.Application.CQRS.Chats.Queries.GetChatList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

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

    [HttpGet("{chatId:guid}")]
    public async Task<ActionResult<ChatDetailsVm>> GetDetails(Guid chatId)
    {
        var query = new GetChatDetailsQuery()
        {
            ResidentId = UserId,
            ChatId = chatId
        };

        var chat = await Mediator.Send(query);

        return Ok(chat);
    }

    [HttpGet("company-chats/{companyId:guid}")]
    public async Task<ActionResult<ChatListForCompanyVm>> GetChatsForCompany(Guid companyId)
    {
        var query = new GetChatForCompanyQuery()
        {
            CompanyId = companyId
        };

        var chats = await Mediator.Send(query);

        return Ok(chats);
    }

    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create(CreateChatDto dto)
    {
        var userName = User.FindFirst(ClaimTypes.Email)?.Value ??
                   User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   "Unknown User";

        var command = mapper.Map<CreateChatCommand>(dto);
        command.ResidentId = UserId;
        command.ResidentName = userName;

        var chatId = await Mediator.Send(command);

        return Ok(chatId);
    }
}
