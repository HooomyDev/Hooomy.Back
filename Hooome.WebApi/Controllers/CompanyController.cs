using AutoMapper;
using Hooome.Application.CQRS.Companies.Commands.CreateCompany;
using Hooome.Application.CQRS.Companies.Commands.UploadLogo;
using Hooome.Application.CQRS.Companies.Queries.GetCompanyDetails;
using Hooome.Application.CQRS.Companies.Queries.GetCompanyList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/companies")]
public class CompanyController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<CompanyListVm>> GetAll()
    {
        var query = new GetCompanyListQuery();

        var companies = await Mediator.Send(query);

        return Ok(companies);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CompanyDetailsVm>> Get(Guid id)
    {
        var query = new GetCompanyDetailsQuery()
        {
            CompanyId = id
        };

        var company = await Mediator.Send(query);

        return Ok(company);
    }

    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCompanyDto dto)
    {
        var command = mapper.Map<CreateCompanyCommand>(dto);

        var companyId = await Mediator.Send(command);

        return Ok(companyId);
    }

    [HttpPost("upload-image")]
    public async Task<ActionResult> UploadLogo([FromForm] IFormFile logo, [FromQuery] Guid companyId)
    {
        var command = new UploadLogoCommand()
        {
            CompanyId = companyId,
            File = logo
        };

        await Mediator.Send(command);

        return NoContent();
    }
}
