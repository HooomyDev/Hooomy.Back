using AutoMapper;
using Hooome.Application.CQRS.Companies.Commands.AddAddress;
using Hooome.Application.CQRS.Companies.Commands.CreateCompany;
using Hooome.Application.CQRS.Companies.Commands.DeleteCompany;
using Hooome.Application.CQRS.Companies.Commands.RemoveAddress;
using Hooome.Application.CQRS.Companies.Commands.UpdateCompany;
using Hooome.Application.CQRS.Companies.Commands.UploadLogo;
using Hooome.Application.CQRS.Companies.Queries.GetCompanyDetails;
using Hooome.Application.CQRS.Companies.Queries.GetCompanyList;
using Hooome.Application.CQRS.Companies.Queries.GetCompanyStatistics;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/companies")]
public class CompanyController(IMapper mapper) : BaseController
{
    [Authorize(Policy = "UserPendingOrGuest")]
    [HttpGet]
    public async Task<ActionResult<CompanyListVm>> GetAll()
    {
        var query = new GetCompanyListQuery();

        var companies = await Mediator.Send(query);

        return Ok(companies);
    }

    [Authorize(Policy = "UserPendingOrGuest")]
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

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCompanyDto dto)
    {
        var command = mapper.Map<CreateCompanyCommand>(dto);

        var companyId = await Mediator.Send(command);

        return Ok(companyId);
    }

    [Authorize(Policy = "AdminOnly")]
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

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{companyId:guid}/add-address/{addressId:guid}")]
    public async Task<ActionResult> AddAddress(Guid companyId, Guid addressId)
    {
        var command = new AddAddressCommand()
        {
            CompanyId = companyId,
            AddressId = addressId
        };

        await Mediator.Send(command);

        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{companyId:guid}/remove-address/{addressId:guid}")]
    public async Task<ActionResult> RemoveAddress(Guid companyId, Guid addressId)
    {
        var command = new RemoveAddressCommand()
        {
            CompanyId = companyId,
            AddressId = addressId
        };

        await Mediator.Send(command);

        return NoContent();
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] UpdateCompanyDto dto)
    {
        var command = mapper.Map<UpdateCompanyCommand>(dto);

        await Mediator.Send(command);

        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("delete/{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteCompanyCommand()
        {
            CompanyId = id,
        };

        await Mediator.Send(command);

        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("statistic")]
    public async Task<ActionResult<CompanyStatisticsVm>> GetStatistic()
    {
        var query = new GetCompanyStatisticsQuery();

        var statistic = await Mediator.Send(query);

        return Ok(statistic);
    }
}
