using AutoMapper;
using Hooome.Application.CQRS.FavoriteAddresses.Commands.CreateFavoriteAddress;
using Hooome.Application.CQRS.FavoriteAddresses.Commands.DeleteFavoriteAddress;
using Hooome.Application.CQRS.FavoriteAddresses.Commands.UpdateFavoriteAddress;
using Hooome.Application.CQRS.FavoriteAddresses.Queries.GetFavoriteAddressList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/favorite-addresses")]
[Authorize]
public class FavoriteAddressController(IMapper mapper) : BaseController
{
    [Authorize(Policy = "ApprovedOnly")]
    [HttpGet]
    public async Task<ActionResult<FavoriteAddressListVm>> Get()
    {
        var query = new GetFavoriteAddressListQuery()
        {
            UserId = UserId,
        };

        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateFavoriteAddressDto dto)
    {
        var command = mapper.Map<CreateFavoriteAddressCommand>(dto);
        command.UserId = UserId;

        var favAddressId = await Mediator.Send(command);

        return Ok(favAddressId);
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] UpdateFavoriteAddressDto dto)
    {
        var command = mapper.Map<UpdateFavoriteAddressCommand>(dto);
        command.UserId = UserId;

        await Mediator.Send(command);

        return NoContent();
    }

    [Authorize(Policy = "ApprovedOnly")]
    [HttpDelete("delete/{id}")]
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