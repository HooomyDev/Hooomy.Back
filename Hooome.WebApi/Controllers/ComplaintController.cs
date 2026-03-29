using AutoMapper;
using Hooome.Application.CQRS.Complaints.Commands.CreateComplaint;
using Hooome.Application.CQRS.Complaints.Commands.DeleteComplaint;
using Hooome.Application.CQRS.Complaints.Commands.UpdateComplaint;
using Hooome.Application.CQRS.Complaints.Queries.GetComplaintCount;
using Hooome.Application.CQRS.Complaints.Queries.GetComplaintDetails;
using Hooome.Application.CQRS.Complaints.Queries.GetComplaintList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/complaints")]
[Authorize]
public class ComplaintsController(IMapper mapper) : BaseController
{

    /// <summary>
    /// Returns list of complaints for current user
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet]
    [ProducesResponseType(typeof(ComplaintListVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ComplaintListVm>> Get()
    {
        var query = new GetComplaintListQuery
        {
            UserId = UserId
        };

        var complaints = await Mediator.Send(query);
        return Ok(complaints);
    }

    /// <summary>
    /// Returns complaint details by id
    /// </summary>
    /// <param name="id">Complaint ID (GUID)</param>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Complaint not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ComplaintDetailsVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ComplaintDetailsVm>> Get(Guid id)
    {
        var query = new GetComplaintDetailsQuery
        {
            UserId = UserId,
            Id = id
        };

        var complaint = await Mediator.Send(query);
        return Ok(complaint);
    }

    /// <summary>
    /// Returns total number of complaints
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("count")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> GetComplaintCount()
    {
        var query = new GetComplaintCountQuery();
        var count = await Mediator.Send(query);
        return Ok(count);
    }

    /// <summary>
    /// Creates new complaint
    /// </summary>
    /// <param name="dto">Complaint data</param>
    /// <returns>Created complaint ID</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Bad request (validation error)</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateComplaintDto dto)
    {
        var command = mapper.Map<CreateComplaintCommand>(dto);
        var complaintId = await Mediator.Send(command);
        return Ok(complaintId);
    }

    /// <summary>
    /// Updates existing complaint
    /// </summary>
    /// <param name="dto">Updated complaint data</param>
    /// <response code="204">Success (no content)</response>
    /// <response code="400">Bad request (validation error)</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Complaint not found</response>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] UpdateComplaintDto dto)
    {
        var command = mapper.Map<UpdateComplaintCommand>(dto);
        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes complaint by id
    /// </summary>
    /// <param name="id">Complaint ID (GUID)</param>
    /// <response code="204">Success (no content)</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden (not your complaint)</response>
    /// <response code="404">Complaint not found</response>
    [HttpDelete("delete/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteComplaintCommand
        {
            Id = id,
            UserId = UserId
        };

        await Mediator.Send(command);
        return NoContent();
    }
}