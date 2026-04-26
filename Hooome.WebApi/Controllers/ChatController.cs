using AutoMapper;
using Hooome.Application.CQRS.Chats.Commands.CreateChat;
using Hooome.Application.CQRS.Chats.Queries.GetChatDetailsQuery;
using Hooome.Application.CQRS.Chats.Queries.GetChatForCompany;
using Hooome.Application.CQRS.Chats.Queries.GetChatList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hooome.WebApi.Controllers;

[Authorize]
[Route("api/chats")]
public class ChatController(IMapper mapper) : BaseController
{
    ///<summary>
    ///Gets all chats for current user
    ///</summary>
    ///<remarks>
    /// | Parameter | Type | Description |
    /// |-----------|------|-------------|
    /// | - | - | - |
    /// 
    /// Example request:
    /// ```
    /// GET /api/chats
    /// 
    /// response:
    /// {
    ///     chats: [
    ///         id: 000000-000000-000000-000000,
    ///         companyName: string
    ///         lastMessageContent: string
    ///         lastMessageSentAt: string 
    ///         createdAt: string
    ///         updatedAt: string
    ///         unreadCount: 0
    ///     ]
    /// }
    /// ```
    ///</remarks>
    ///<response code="200">Ok</response>
    ///<response code="401">Unauthorized</response>
    [Authorize(Policy = "ApprovedOnly")]
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

    ///<summary>
    ///Gets chat details by id
    ///</summary>
    ///<remarks>
    /// | Parameter | Type | Description |
    /// |-----------|------|-------------|
    /// | chatId | guid | Chat identifier |
    /// 
    /// Example request:
    /// ```
    /// GET /api/chats/000000-000000-000000-000000
    /// 
    /// response:
    /// {
    ///     id: 000000-000000-000000-000000,
    ///     residentName: string,
    ///     companyName: string,
    ///     status: 0,
    ///     messages: [
    ///         id: 000000-000000-000000-000000,
    ///         text: string,
    ///         senderName: string,
    ///         createdAt: datetime
    ///     ]
    /// }
    /// ```
    ///</remarks>
    ///<response code="200">Ok</response>
    ///<response code="401">Unauthorized</response>
    ///<response code="404">Not found</response>
    [Authorize(Policy = "ApprovedOnly")]
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

    ///<summary>
    ///Gets all chats for company
    ///</summary>
    ///<remarks>
    /// | Parameter | Type | Description |
    /// |-----------|------|-------------|
    /// | companyId | guid | Company identifier |
    /// 
    /// Example request:
    /// ```
    /// GET /api/chats/company-chats/000000-000000-000000-000000
    /// 
    /// response:
    /// {
    ///     chats: [
    ///         id: 000000-000000-000000-000000,
    ///         companyName: string
    ///         lastMessageContent: string
    ///         lastMessageSentAt: string 
    ///         createdAt: string
    ///         updatedAt: string
    ///         unreadCount: 0
    ///     ]
    /// }
    /// ```
    ///</remarks>
    ///<response code="200">Ok</response>
    ///<response code="401">Unauthorized</response>
    [Authorize(Policy = "EmployeeOnly")]
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

    ///<summary>
    ///Creates new chat
    ///</summary>
    ///<remarks>
    /// Example request:
    /// ```
    /// POST /api/chats/create
    /// 
    /// request body:
    /// {
    ///     companyId: "000000-000000-000000-000000"
    /// }
    /// 
    /// response:
    /// "000000-000000-000000-000000"
    /// ```
    ///</remarks>
    ///<response code="200">Ok</response>
    ///<response code="400">Bad request</response>
    ///<response code="401">Unauthorized</response>
    [Authorize(Policy = "ApprovedOnly")]
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
