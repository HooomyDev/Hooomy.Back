using AutoMapper;
using Hooome.Application.CQRS.Complaints.Commands.CreateComplaint;
using Hooome.Application.CQRS.Complaints.Commands.DeleteComplaint;
using Hooome.Application.CQRS.Complaints.Commands.UpdateComplaint;
using Hooome.Application.CQRS.Complaints.Queries.GetComplaintCount;
using Hooome.Application.CQRS.Complaints.Queries.GetComplaintDetails;
using Hooome.Application.CQRS.Complaints.Queries.GetComplaintList;
using Hooome.Domain.Enums;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/complaints")]
[Authorize]
public class ComplaintsController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<ComplaintListVm>> Get(
            [FromQuery] ComplaintStatus status = ComplaintStatus.Unknown,
            [FromQuery] ComplaintType type = ComplaintType.Unknown,
            [FromQuery] string shortDescription = "")
    {
        var query = new GetComplaintListQuery
        {
            Status = status,
            Type = type,
            ShortDescription = shortDescription,
        };

        var complaints = await Mediator.Send(query);
        return Ok(complaints);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ComplaintDetailsVm>> Get(Guid id)
    {
        var query = new GetComplaintDetailsQuery
        {
            Id = id
        };

        var complaint = await Mediator.Send(query);
        return Ok(complaint);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetComplaintCount()
    {
        var query = new GetComplaintCountQuery();
        var count = await Mediator.Send(query);
        return Ok(count);
    }

    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateComplaintDto dto)
    {
        var command = mapper.Map<CreateComplaintCommand>(dto);
        var complaintId = await Mediator.Send(command);
        return Ok(complaintId);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateComplaintDto dto)
    {
        var command = mapper.Map<UpdateComplaintCommand>(dto);
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteComplaintCommand
        {
            Id = id,
        };

        await Mediator.Send(command);
        return NoContent();
    }
}