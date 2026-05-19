using AutoMapper;
using Hooome.Application.CQRS.RequestComments.Commands.CreateComment;
using Hooome.Application.CQRS.RequestComments.Commands.DeleteComment;
using Hooome.Application.CQRS.RequestComments.Commands.UpdateComment;
using Hooome.Application.CQRS.RequestComments.Commands.UploadCommentImages;
using Hooome.Application.CQRS.RequestComments.Queries.GetRequestCommentCount;
using Hooome.Application.CQRS.RequestComments.Queries.GetRequestComments;
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
    [Authorize(Policy = "ApprovedOnly")]
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

    [Authorize(Policy = "ApprovedOnly")]
    [HttpGet("{id}")]
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

    [Authorize(Policy = "UserPendingOrGuest")]
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var query = new GetRequestCountQuery();

        var count = await Mediator.Send(query);

        return Ok(count);
    }

    [Authorize(Policy = "UserPendingOrGuest")]
    [HttpGet("statistic")]
    public async Task<ActionResult<RequestDailyStatisticVm>> Get([FromQuery] int period,
        [FromQuery] Guid? companyId)
    {
        var query = new GetRequestDailyStatisticQuery
        {
            Period = (RequestsPeriod)period,
            CompanyId = companyId
        };

        var statistic = await Mediator.Send(query);

        return Ok(statistic);
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpGet("categories")]
    public async Task<ActionResult<RequestCategoryListVm>> GetCategories()
    {
        var query = new GetRequestCategoryListQuery();

        var categories = await Mediator.Send(query);

        return Ok(categories);
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateRequestDto dto)
    {
        var command = mapper.Map<CreateRequestCommand>(dto);
        command.UserId = UserId;

        var requestId = await Mediator.Send(command);

        return Ok(requestId);
    }

    [Authorize(Policy = "ApprovedOnly")]
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

    [Authorize(Policy = "ApprovedOnly")]
    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] UpdateRequestDto dto)
    {
        var command = mapper.Map<UpdateRequestCommand>(dto);

        await Mediator.Send(command);

        return NoContent();
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpDelete("delete/{id}")]
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

    [Authorize(Policy = "UserPendingOrGuest")]
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

    [Authorize(Policy = "AdminOrEmployeeOnly")]
    [HttpGet("administration")]
    public async Task<ActionResult<RequestListWithPaginationVm>> GetRequests(
        [FromQuery] string? title,
        [FromQuery] Guid? companyId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] RequestCategory category = RequestCategory.None,
        [FromQuery] RequestStatus status = RequestStatus.Unknown)
    {
        var query = new GetRequestListWithPaginationQuery
        {
            Title = title,
            CompanyId = companyId,
            Page = page,
            PageSize = pageSize,
            Category = category,
            Status = status
        };

        var requests = await Mediator.Send(query);

        return Ok(requests);
    }

    [Authorize(Policy = "AdminOrEmployeeOnly")]
    [HttpPost("add-comment")]
    public async Task<ActionResult<Guid>> AddComment([FromBody] AddCommentDto dto)
    {
        var command = mapper.Map<CreateRequestCommentCommand>(dto);
        command.UserId = UserId;

        var commentId = await Mediator.Send(command);

        return Ok(commentId);
    }

    [Authorize(Policy = "AdminOrEmployeeOnly")]
    [HttpPost("comments/{commentId:guid}/upload-comment-images")]
    public async Task<ActionResult> UploadCommentImages(Guid commentId, [FromForm] List<IFormFile> files)
    {
        var command = new UploadRequestCommentImageCommand
        {
            RequestCommentId = commentId,
            Files = files
        };

        await Mediator.Send(command);

        return NoContent();
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpGet("comments")]
    public async Task<ActionResult<RequestCommentsVm>> GetComments([FromQuery] Guid? requestId,
        [FromQuery] string? text,
        [FromQuery] RequestCommentStatus? status,
        [FromQuery] int page = 1,
        int pageSize = 5)
    {
        var query = new GetRequestCommentsQuery
        {
            RequestId = requestId,
            Text = text,
            Status = status,
            Page = page,
            PageSize = pageSize
        };

        var comments = await Mediator.Send(query);

        return Ok(comments);
    }

    [Authorize(Policy = "AdminOrEmployeeOnly")]
    [HttpGet("comments/count")]
    public async Task<ActionResult<int>> GetCommentCount([FromQuery] Guid? requestId, string filter = "all")
    {
        var query = new GetRequestCommentCountQuery
        {
            RequestId = requestId,
            Filter = filter
        };

        var count = await Mediator.Send(query);

        return Ok(count);
    }

    [Authorize(Policy = "AdminOrEmployeeOnly")]
    [HttpDelete("comments/delete/{commentId:guid}")]
    public async Task<ActionResult> DeleteComment(Guid commentId)
    {
        var command = new DeleteRequestCommentCommand
        {
            CommentId = commentId
        };

        await Mediator.Send(command);

        return NoContent();
    }

    [Authorize(Policy = "AdminOrEmployeeOnly")]
    [HttpPut("comments/update")]
    public async Task<ActionResult> UpdateComment([FromBody] UpdateCommentDto dto)
    {
        var query = mapper.Map<UpdateRequestCommentCommand>(dto);

        await Mediator.Send(query);

        return NoContent();
    }
}
