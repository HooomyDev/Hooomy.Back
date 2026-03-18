using Hooome.Application.Companies.Queries.GetCompanyList;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/companies")]
public class CompanyController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<CompanyListVm>> Get()
    {
        var query = new GetCompanyListQuery();

        var companies = await Mediator.Send(query);

        return Ok(companies);
    }
}
