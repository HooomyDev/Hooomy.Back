using AutoMapper;
using Hooome.Application.CQRS.Requests.Commands.CreateRequest;
using Hooome.Application.CQRS.Requests.Commands.DeleteRequest;
using Hooome.Application.CQRS.Requests.Commands.UpdateRequest;
using Hooome.Application.CQRS.Requests.Commands.UploadImages;
using Hooome.Application.CQRS.Requests.Queries.GetMapData;
using Hooome.Application.CQRS.Requests.Queries.GetRequestCategoryList;
using Hooome.Application.CQRS.Requests.Queries.GetRequestCount;
using Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;
using Hooome.Application.CQRS.Requests.Queries.GetRequestDetails;
using Hooome.Application.CQRS.Requests.Queries.GetRequestList;
using Hooome.Application.CQRS.Requests.Queries.GetRequestListWithPagination;
using Hooome.Domain.Enums;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

/// <summary>
/// Manages request operations including retrieval, creation, updating, and deletion
/// </summary>
[Route("api/requests")]
public class RequestController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<RequestListVm>> Get(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] RequestStatus requestStatus = RequestStatus.Unknown
        )
    {
        var query = new GetRequestListQuery
        {
            UserId = UserId,
            StartDate = startDate,
            EndDate = endDate,
            RequestStatus = requestStatus
        };

        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RequestDetailsVm>> Get(Guid id)
    {
        var query = new GetRequestDetailsQuery
        {
            UserId = UserId,
            Id = id
        };

        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

    [HttpGet("count")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> GetCount()
    {
        var query = new GetRequestCountQuery();

        var count = await Mediator.Send(query);

        return Ok(count);
    }

    [HttpGet("statistic")]
    public async Task<ActionResult<RequestDailyStatisticVm>> Get([FromQuery] int period)
    {
        var query = new GetRequestDailyStatisticQuery
        {
            Period = (RequestsPeriod)period
        };

        var statistic = await Mediator.Send(query);

        return Ok(statistic);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<RequestCategoryListVm>> GetCategories()
    {
        var query = new GetRequestCategoryListQuery();

        var categories = await Mediator.Send(query);

        return Ok(categories);
    }

    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateRequestDto dto)
    {
        var command = mapper.Map<CreateRequestCommand>(dto);
        command.UserId = UserId;

        var requestId = await Mediator.Send(command);

        return Ok(requestId);
    }

    [Authorize]
    [HttpPost("{requestId:guid}/upload-images")]
    public async Task<ActionResult> UploadImages(Guid requestId, [FromForm] List<IFormFile> files)
    {
        var command = new UploadImagesCommand()
        {
            RequestId = requestId,
            UserId = UserId,
            Files = files
        };

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("update")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromBody] UpdateRequestDto dto)
    {
        var command = mapper.Map<UpdateRequestCommand>(dto);
        command.UserId = UserId;

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteRequestCommand
        {
            Id = id,
            UserId = UserId
        };

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpGet("map")]
    public async Task<ActionResult<MapDataVm>> GetClusters(
        [FromQuery] int zoom = 12,
        [FromQuery] int month = 0,
        [FromQuery] RequestStatus status = RequestStatus.Unknown)
    {
        var query = new GetMapDataQuery()
        {
            ZoomLevel = zoom,
            Month = month,
            RequestStatus = status
        };

        var result = await Mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("administration")]
    public async Task<ActionResult<RequestListWithPaginationVm>> GetRequests(
        [FromQuery] string? title,
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] RequestCategory category = RequestCategory.None,
        [FromQuery] RequestStatus status = RequestStatus.Unknown)
    {
        var query = new GetRequestListWithPaginationQuery
        {
            Title = title,
            Page = page,
            PageSize = pageSize,
            Category = category,
            Status = status
        };

        var requests = await Mediator.Send(query);

        return Ok(requests);
    }
}