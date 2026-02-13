using Hooome.Application.Streets.Queries.GetStreetList;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

/// <summary>
/// Handles search operations for streets and addresses
/// </summary>
/// <remarks>
/// This controller provides endpoint for searching streets based on query parameters.
/// </remarks>
[Route("api/search")]
public class StreetController : BaseController
{
    /// <summary>
    /// Searches for streets based on the provided query string
    /// </summary>
    /// <remarks>
    /// This endpoint performs a fuzzy search on street names and returns matching results.
    /// Useful for implementing address autocomplete in frontend applications.
    /// 
    /// Sample request:
    /// 
    ///     GET /api/search?query=main
    ///     
    ///     GET /api/search?query=lenina
    ///     
    ///     GET /api/search?query=prospekt%20mira
    /// 
    /// </remarks>
    /// <param name="query">The search query string (minimum 2 characters recommended)</param>
    /// <returns>A list of streets matching the search query</returns>
    /// <response code="200">Returns the list of matching streets</response>
    /// <response code="400">If the query parameter is missing or invalid</response>
    /// <response code="401">If user is unauthorized (if authorization is required)</response>
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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