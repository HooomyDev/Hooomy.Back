using AutoMapper;
using Hooome.Application.CQRS.Inquiries.Commands.CreateInquire;
using Hooome.Application.CQRS.Inquiries.Queries.GetInquiryList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[ApiController]
[Route("api/inquiries")]
public class InquiryController(IMapper mapper) : BaseController
{
    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public async Task<ActionResult<InquiryListVm>> Get(
        [FromQuery] DateTime? date,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
        )
    {
        var query = new GetInquiryListQuery
        {
            Date = date,
            Page = page,
            PageSize = pageSize
        };

        var inquiries = await Mediator.Send(query);

        return Ok(inquiries);
    }

    [Authorize(Policy = "UserPendingOrGuest")]
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateInquiryDto dto)
    {
        var command = mapper.Map<CreateInquiryCommand>(dto);

        var inquiryId = await Mediator.Send(command);

        return Ok(inquiryId);
    }
}
