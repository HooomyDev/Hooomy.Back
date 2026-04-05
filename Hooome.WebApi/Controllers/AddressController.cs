using Hooome.Application.CQRS.Addresses.Commands.CreateAddress;
using Hooome.Application.CQRS.Addresses.Queries.GetAddressList;
using Hooome.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpGet("find-or-create")]
    public async Task<ActionResult<Guid>> FindOrCreateAddress(
        [FromQuery] double lat,
        [FromQuery] double lng,
        [FromQuery] string address)
    {
        var query = new CreateAddressCommand()
        {
            Latitude = lat,
            Longitude = lng,
            Address = address
        };

        var id = await Mediator.Send(query);

        return Ok(id);
    }
}
