using Hooome.Application.Streets.Queries.GetStreetList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/search")]
public class StreetController : BaseController
{
    [HttpGet()]
    public async Task<ActionResult<StreetListVm>> Get([FromQuery] string query)
    {
        var Query = new GetStreetListQuery
        {
            Query = query
        };

        var streets = await Mediator.Send(Query);

        return Ok(streets);
    }
}
