using AutoMapper;
using Hooome.Application.CQRS.Polls.Commands.CreatePoll;
using Hooome.Application.CQRS.Polls.Commands.DeletePoll;
using Hooome.Application.CQRS.Polls.Commands.SubmitVote;
using Hooome.Application.CQRS.Polls.Commands.UpdatePoll;
using Hooome.Application.CQRS.Polls.Queries.GetPollDetails;
using Hooome.Application.CQRS.Polls.Queries.GetPollList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/polls")]
[Authorize]
public class PollController(IMapper mapper) : BaseController
{
    [Authorize(Policy = "UserPendingOrGuest")]
    [HttpGet]
    public async Task<ActionResult<PollListVm>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string filter = "all")
    {
        var query = new GetPollListQuery()
        {
            Page = page,
            PageSize = pageSize,
            FilterOption = filter
        };

        var polls = await Mediator.Send(query);

        return Ok(polls);
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpGet("{pollId:guid}")]
    public async Task<ActionResult<PollDetailsVm>> GetDetails(Guid pollId)
    {
        var query = new GetPollDetailsQuery()
        {
            Id = pollId,
            UserId = UserId
        };

        var poll = await Mediator.Send(query);

        return Ok(poll);
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpPost("{id:guid}/vote")]
    public async Task<ActionResult<PollDetailsVm>> Vote(Guid id, [FromBody] SubmitVoteDto dto)
    {
        var command = new SubmitVoteCommand()
        {
            PollId = id,
            UserId = UserId,
            Vote = dto
        };

        await Mediator.Send(command);

        var query = new GetPollDetailsQuery()
        {
            Id = id,
            UserId = UserId,
        };

        var poll = await Mediator.Send(query);

        return Ok(poll);
    }

    [Authorize(Policy = "EmployeeOnly")]
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreatePollDto dto)
    {
        var command = mapper.Map<CreatePollCommand>(dto);
        command.CreatedBy = UserId;

        var pollId = await Mediator.Send(command);

        return Ok(pollId);
    }

    [Authorize(Policy = "EmployeeOnly")]
    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] UpdatePollDto dto)
    {
        var command = mapper.Map<UpdatePollCommand>(dto);
        command.CreatedBy = UserId;
        
        await Mediator.Send(command);

        return NoContent();
    }

    [Authorize(Policy = "EmployeeOnly")]
    [HttpDelete("delete/{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeletePollCommand()
        {
            Id = id,
            CreatedBy = UserId
        };

        await Mediator.Send(command);

        return NoContent();
    }
}
