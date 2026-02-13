using AutoMapper;
using Hooome.Application.FavoriteAddresses.Commands.CreateFavoriteAddress;
using Hooome.Application.FavoriteAddresses.Commands.DeleteFavoriteAddress;
using Hooome.Application.FavoriteAddresses.Commands.UpdateFavoriteAddress;
using Hooome.Application.FavoriteAddresses.Queries.GetFavoriteAddressList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

/// <summary>
/// Manages user's favorite addresses
/// </summary>
/// <remarks>
/// This controller provides CRUD operations for managing favorite addresses.
/// Users can save, retrieve, update, and delete their frequently used addresses.
/// All endpoints require authorization.
/// </remarks>
[Route("api/favorite-addresses")]
[Authorize]
public class FavoriteAddressController(IMapper mapper) : BaseController
{
    /// <summary>
    /// Retrieves all favorite addresses for the current user
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/favorite-addresses
    /// 
    /// </remarks>
    /// <returns>List of user's favorite addresses</returns>
    /// <response code="200">Returns the list of favorite addresses</response>
    /// <response code="401">If user is unauthorized</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<FavoriteAddressListVm>> Get()
    {
        var query = new GetFavoriteAddressListQuery()
        {
            UserId = UserId,
        };

        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

    /// <summary>
    /// Creates a new favorite address for the current user
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/favorite-addresses/create
    ///     {
    ///         "name": "Home",
    ///         "addressLine1": "123 Main Street",
    ///         "addressLine2": "Apt 4B",
    ///         "city": "New York",
    ///         "state": "NY",
    ///         "postalCode": "10001",
    ///         "country": "USA",
    ///         "latitude": 40.7128,
    ///         "longitude": -74.0060,
    ///         "notes": "Near Central Park",
    ///         "tags": ["home", "primary"],
    ///         "isDefault": true
    ///     }
    /// 
    /// </remarks>
    /// <param name="dto">The favorite address creation data</param>
    /// <returns>The unique identifier of the created favorite address</returns>
    /// <response code="200">Returns the ID of the created favorite address</response>
    /// <response code="400">If the address data is invalid</response>
    /// <response code="401">If user is unauthorized</response>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateFavoriteAddressDto dto)
    {
        var command = mapper.Map<CreateFavoriteAddressCommand>(dto);
        command.UserId = UserId;

        var favAddressId = await Mediator.Send(command);

        return Ok(favAddressId);
    }

    /// <summary>
    /// Updates an existing favorite address
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /api/favorite-addresses/update
    ///     {
    ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "name": "Updated Home",
    ///         "addressLine1": "456 Oak Avenue",
    ///         "addressLine2": "Suite 12",
    ///         "city": "Los Angeles",
    ///         "state": "CA",
    ///         "postalCode": "90210",
    ///         "country": "USA",
    ///         "latitude": 34.0522,
    ///         "longitude": -118.2437,
    ///         "notes": "New office location",
    ///         "tags": ["work", "updated"],
    ///         "isDefault": false
    ///     }
    /// 
    /// </remarks>
    /// <param name="dto">The favorite address update data</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the address was successfully updated</response>
    /// <response code="400">If the update data is invalid</response>
    /// <response code="401">If user is unauthorized</response>
    /// <response code="404">If the favorite address is not found</response>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromBody] UpdateFavoriteAddressDto dto)
    {
        var command = mapper.Map<UpdateFavoriteAddressCommand>(dto);
        command.UserId = UserId;

        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    /// Deletes a specific favorite address
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     DELETE /api/favorite-addresses/delete/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// 
    /// </remarks>
    /// <param name="id">The unique identifier of the favorite address to delete</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the address was successfully deleted</response>
    /// <response code="401">If user is unauthorized</response>
    /// <response code="404">If the favorite address is not found</response>
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteFavoriteAddressCommand
        {
            Id = id,
            UserId = UserId
        };

        await Mediator.Send(command);

        return NoContent();
    }
}