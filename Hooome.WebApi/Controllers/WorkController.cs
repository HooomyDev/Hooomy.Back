using AutoMapper;
using Hooome.Application.Works.Commands.CreateWork;
using Hooome.Application.Works.Queries.GetWorkList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("/api/works")]
public class WorkController(IMapper mapper) : BaseController
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<GetWorkListVm>> Get()
    {
        var query = new GetWorkListQuery
        {
            UserId = UserId
        };

        var works = await Mediator.Send(query);

        return Ok(works);
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateWorkDto dto)
    {
        var command = mapper.Map<CreateWorkCommand>(dto);

        var workId = await Mediator.Send(command);

        return Ok(workId);
    }
}
