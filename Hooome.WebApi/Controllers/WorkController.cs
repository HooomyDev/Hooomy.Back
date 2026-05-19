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
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateWorkDto dto)
    {
        var command = mapper.Map<CreateWorkCommand>(dto);

        var workId = await Mediator.Send(command);

        return Ok(workId);
    }
}