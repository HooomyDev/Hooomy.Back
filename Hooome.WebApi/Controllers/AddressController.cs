using Hooome.Application.CQRS.Addresses.Queries.GetAddressList;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[ApiController]
[Route("api/addresses")]
public class AddressController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<AddressListVm>> Get([FromQuery] string searchQuery)
    {
        var query = new GetAddressListQuery()
        {
            Query = searchQuery
        };

        var addresses = await Mediator.Send(query);

        return Ok(addresses);
    }
}
