using AutoMapper;
using Hooome.Application.Requests.Commands.CreateRequest;
using Hooome.Application.Requests.Commands.DeleteRequest;
using Hooome.Application.Requests.Commands.UpdateRequest;
using Hooome.Application.Requests.Queries.GetRequestDetails;
using Hooome.Application.Requests.Queries.GetRequestList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/requests")]
public class RequestController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<RequestListVm>> Get()
    {
        var query = new GetRequestListQuery
        {
            UserId = UserId
        };

        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

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

    public async Task<ActionResult<Guid>> Create([FromBody] CreateRequestDto dto)
    {
        var command = mapper.Map<CreateRequestCommand>(dto);
        command.UserId = UserId;

        var requestId = await Mediator.Send(command);

        return Ok(requestId);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateRequestDto dto)
    {
        var command = mapper.Map<UpdateRequestCommand>(dto);
        command.UserId = UserId;
    
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
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
