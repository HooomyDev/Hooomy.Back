using Hooome.Application.CQRS.Notifications.Commands.CreateRequestNotification;
using Hooome.Application.CQRS.Notifications.Commands.CreateSystemNotification;
using Hooome.Application.CQRS.Notifications.Commands.CreateWorkNotification;
using AutoMapper;
using Hooome.Application.CQRS.Notifications.Queries.GetNotifications;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/notifications")]
[Authorize]
public class NotificationController(IMapper mapper) : BaseController
{
    [Authorize(Policy = "ApprovedOnly")]
    [HttpGet]
    public async Task<ActionResult<NotificationListVm>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 15)
    {
        var query = new GetNotificationsQuery
        {
            UserId = UserId,
            Page = page,
            PageSize = pageSize
        };

        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpPost("system/create")]
    public async Task<ActionResult<Guid>> CreateSystem([FromBody] CreateSystemNotificationDto dto)
    {
        var command = mapper.Map<CreateSystemNotificationCommand>(dto);

        var notificationId = await Mediator.Send(command);
        
        return Ok(notificationId);
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpPost("request/create")]
    public async Task<ActionResult<Guid>> CreateRequest([FromBody] CreateRequestNotificationDto dto)
    {
        var command = mapper.Map<CreateRequestNotificationCommand>(dto);

        var notificationId = await Mediator.Send(command);
        
        return Ok(notificationId);
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpPost("work/create")]
    public async Task<ActionResult<Guid>> CreateWork([FromBody] CreateWorkNotificationDto dto)
    {
        var command = mapper.Map<CreateWorkNotificationCommand>(dto);
        
        var notificationId = await Mediator.Send(command);
        
        return Ok(notificationId);
    }
}
