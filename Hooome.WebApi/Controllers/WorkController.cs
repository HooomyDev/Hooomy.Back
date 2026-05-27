using AutoMapper;
using Hooome.Application.CQRS.Requests.Queries.GetRequestListWithPagination;
using Hooome.Application.CQRS.Works.Commands.CreateWork;
using Hooome.Application.CQRS.Works.Commands.DeleteWork;
using Hooome.Application.CQRS.Works.Commands.UpdateWork;
using Hooome.Application.CQRS.Works.Queries.GetWorkList;
using Hooome.Application.CQRS.Works.Queries.GetWorkListForEmployee;
using Hooome.Domain;
using Hooome.Domain.Enums;
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
    [Authorize(Policy = "ApprovedOnly")]
    [HttpGet]
    public async Task<ActionResult<GetWorkListVm>> Get()
    {
        var query = new GetWorkListQuery
        {
            UserId = UserId
        };

        var works = await Mediator.Send(query);

        return Ok(works);
    }

    [Authorize(Policy = "AdminOrEmployeeOnly")]
    [HttpGet("get-for-employee")]
    public async Task<ActionResult<GetWorkListWithPaginationVm>> GetForEmployee(
        [FromQuery] RequestCategory? category,
        [FromQuery] WorkSeriousness? seriousness,
        [FromQuery] Guid? addressId,
        [FromQuery] string? searchTitle,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
        )
    {
        var query = new GetWorkListForEmployeeQuery
        {
            Category = category,
            Seriousness = seriousness,
            AddressId = addressId,
            SearchTitle = searchTitle,
            PageSize = pageSize,
            Page = page
        };

        var works = await Mediator.Send(query);

        return Ok(works);
    }

    [Authorize(Policy = "AdminOrEmployeeOnly")]
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateWorkDto dto)
    {
        var command = mapper.Map<CreateWorkCommand>(dto);

        var workId = await Mediator.Send(command);

        return Ok(workId);
    }

    [Authorize(Policy = "AdminOrEmployeeOnly")]
    [HttpPut("update")]
    public async Task<ActionResult<Guid>> Update([FromBody] UpdateWorkDto dto)
    {
        var command = mapper.Map<UpdateWorkCommand>(dto);

        await Mediator.Send(command);

        return NoContent();
    }

    [Authorize(Policy = "AdminOrEmployeeOnly")]
    [HttpDelete("delete/{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(Guid id)
    {
        var command = new DeleteWorkCommand
        {
            WorkId = id
        };

        await Mediator.Send(command);

        return NoContent();
    }
}
