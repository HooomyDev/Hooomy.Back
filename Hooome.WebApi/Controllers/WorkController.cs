using AutoMapper;
using Hooome.Application.CQRS.Works.Commands.CreateWork;
using Hooome.Application.CQRS.Works.Queries.GetWorkList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

/// <summary>
/// Manages work items operations including retrieval and creation
/// </summary>
[Route("/api/works")]
public class WorkController(IMapper mapper) : BaseController
{
    /// <summary>
    /// Retrieves all work items for the current user
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/works
    /// 
    /// </remarks>
    /// <returns>List of work items belonging to the current user</returns>
    /// <response code="200">Returns the list of work items</response>
    /// <response code="401">If user is unauthorized</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetWorkListVm>> Get()
    {
        var query = new GetWorkListQuery
        {
            UserId = UserId
        };

        var works = await Mediator.Send(query);

        return Ok(works);
    }

    /// <summary>
    /// Creates a new work item
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/works/create
    ///     {
    ///         "title": "Complete project documentation",
    ///         "description": "Write comprehensive documentation for the Work module",
    ///         "priority": "High",
    ///         "dueDate": "2024-12-31T00:00:00Z",
    ///         "estimatedHours": 8,
    ///         "tags": ["documentation", "urgent"]
    ///     }
    /// 
    /// </remarks>
    /// <param name="dto">The work item creation data transfer object</param>
    /// <returns>The unique identifier of the created work item</returns>
    /// <response code="200">Returns the ID of the created work item</response>
    /// <response code="400">If the work item data is invalid</response>
    /// <response code="401">If user is unauthorized</response>
    [HttpPost("create")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateWorkDto dto)
    {
        var command = mapper.Map<CreateWorkCommand>(dto);

        var workId = await Mediator.Send(command);

        return Ok(workId);
    }
}