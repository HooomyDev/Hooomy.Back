using AutoMapper;
using Hooome.Application.CQRS.Requests.Commands.CreateRequest;
using Hooome.Application.CQRS.Requests.Queries.GetRequestCateryList;
using Hooome.Application.Requests.Commands.DeleteRequest;
using Hooome.Application.Requests.Commands.UpdateRequest;
using Hooome.Application.Requests.Queries.GetRequestCount;
using Hooome.Application.Requests.Queries.GetRequestDailyStatistics;
using Hooome.Application.Requests.Queries.GetRequestDetails;
using Hooome.Application.Requests.Queries.GetRequestList;
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
    /// <summary>
    /// Retrieves all requests for the current user
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/requests
    /// 
    /// </remarks>
    /// <returns>List of requests belonging to the current user</returns>
    /// <response code="200">Returns the list of requests</response>
    /// <response code="401">If user is unauthorized</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RequestListVm>> Get()
    {
        var query = new GetRequestListQuery
        {
            UserId = UserId
        };

        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

    /// <summary>
    /// Retrieves detailed information about a specific request
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/requests/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// 
    /// </remarks>
    /// <param name="id">The unique identifier of the request (GUID)</param>
    /// <returns>Detailed information about the requested request</returns>
    /// <response code="200">Returns the request details</response>
    /// <response code="401">If user is unauthorized</response>
    /// <response code="404">If request with specified ID is not found</response>
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

    /// <summary>
    /// Retrieves the total count of requests in the system
    /// </summary>
    /// <remarks>
    /// This endpoint provides administrative insights by returning the total number of requests.
    /// It can be used for dashboard metrics, reporting, or monitoring purposes.
    /// 
    /// Sample request:
    /// 
    ///     GET /api/requests/count
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "totalCount": 1542,
    ///         "timestamp": "2024-01-15T10:30:45Z"
    ///     }
    /// 
    /// </remarks>
    /// <returns>The total number of requests in the system</returns>
    /// <response code="200">Returns the total count of requests</response>
    /// <response code="401">If user is unauthorized</response>
    /// <response code="403">If user is not an admin</response>
    /// <response code="500">If there was an internal server error</response>
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

    /// <summary>
    /// Creates a new request
    /// </summary>
    /// <param name="dto">The request creation data transfer object</param>
    /// <returns>The unique identifier of the created request</returns>
    /// <response code="200">Returns the ID of the created request</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="401">If user is unauthorized</response>
    [HttpPost("create")]
    [Authorize]
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

    /// <summary>
    /// Updates an existing request
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /api/requests/update
    ///     {
    ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "title": "Updated Request Title",
    ///         "description": "Updated description",
    ///         "priority": "Low",
    ///         "status": "InProgress",
    ///         "dueDate": "2025-01-15T00:00:00Z"
    ///     }
    /// 
    /// </remarks>
    /// <param name="dto">The request update data transfer object</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the request was successfully updated</response>
    /// <response code="400">If the update data is invalid</response>
    /// <response code="401">If user is unauthorized</response>
    /// <response code="404">If request with specified ID is not found</response>
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

    /// <summary>
    /// Deletes a specific request
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     DELETE /api/requests/delete/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// 
    /// </remarks>
    /// <param name="id">The unique identifier of the request to delete (GUID)</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the request was successfully deleted</response>
    /// <response code="401">If user is unauthorized</response>
    /// <response code="404">If request with specified ID is not found</response>
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
}