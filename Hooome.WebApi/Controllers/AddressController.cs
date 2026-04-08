using Hooome.Application.CQRS.Addresses.Commands.CreateAddress;
using Hooome.Application.CQRS.Addresses.Queries.GetAddressList;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[ApiController]
[Route("api/addresses")]
[Produces("application/json")]
[Consumes("application/json")]
public class AddressController : BaseController
{
    ///<summary>
    ///Gets addresses by search query
    ///</summary>
    ///<remarks>
    /// | Parameter | Type | Description |
    /// |-----------|------|-------------|
    /// | searchQuery | string | search query |
    /// 
    /// Example request:
    /// ```
    /// GET /api/addresses?searchQuery=street
    /// 
    /// response:
    /// {
    ///     addresses: [
    ///         id: 000000-000000-000000-000000,
    ///         street: string,
    ///         houseNumber: string
    ///     ]
    /// }
    /// ```
    /// </remarks>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet]
    [ProducesResponseType(typeof(AddressListVm), StatusCodes.Status200OK)]
    public async Task<ActionResult<AddressListVm>> Get([FromQuery] string searchQuery)
    {
        var query = new GetAddressListQuery()
        {
            Query = searchQuery
        };

        var addresses = await Mediator.Send(query);

        return Ok(addresses);
    }

    /// <summary>
    /// Finds an existing address by coordinates or creates a new one
    /// </summary>
    /// <param name="lat">Latitude in degrees (range: -90 to 90)</param>
    /// <param name="lng">Longitude in degrees (range: -180 to 180)</param>
    /// <param name="address">Full address in text format</param>
    /// <remarks>
    /// ## Parameters
    /// | Parameter | Type | Description | Example |
    /// |-----------|------|-------------|---------|
    /// | lat | double | Latitude (range: -90 to 90) | 53.9023 |
    /// | lng | double | Longitude (range: -180 to 180) | 27.5619 |
    /// | address | string | Full address text | "Street, 34a" |
    /// 
    /// Example request:
    /// 
    /// ```
    /// GET /api/addresses/find-or-create?lat=53.9023&amp;lng=27.5619&amp;address=15%20Lenin%20Street%2C%20Minsk
    /// 
    /// "123e4567-e89b-12d3-a456-426614174000"
    /// ```
    /// </remarks>
    /// <response code="200">Address found or created successfully. Returns the address GUID</response>
    /// <response code="400">Invalid parameters (invalid coordinates, empty address, etc.)</response>
    /// <response code="401">User is not authenticated</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("find-or-create")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
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
