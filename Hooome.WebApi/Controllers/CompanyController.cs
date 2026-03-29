using Hooome.Application.CQRS.Companies.Queries.GetCompanyDetails;
using Hooome.Application.CQRS.Companies.Queries.GetCompanyList;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/companies")]
public class CompanyController : BaseController
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
}
