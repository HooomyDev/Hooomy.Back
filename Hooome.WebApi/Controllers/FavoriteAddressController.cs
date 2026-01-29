using AutoMapper;
using Hooome.Application.FavoriteAddresses.Commands.CreateFavoriteAddress;
using Hooome.Application.FavoriteAddresses.Commands.DeleteFavoriteAddress;
using Hooome.Application.FavoriteAddresses.Commands.UpdateFavoriteAddress;
using Hooome.Application.FavoriteAddresses.Queries.GetFavoriteAddressList;
using Hooome.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Controllers;

[Route("api/favorite-addresses")]
public class FavoriteAddressController(IMapper mapper) : BaseController
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<FavoriteAddressListVm>> Get()
    {
        var query = new GetFavoriteAddressListQuery()
        {
            UserId = UserId,
        };

        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateFavoriteAddressDto dto) 
    {
        var command = mapper.Map<CreateFavoriteAddressCommand>(dto);
        command.UserId = UserId;

        var favAddressId = await Mediator.Send(command);

        return Ok(favAddressId);
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateFavoriteAddressDto dto)
    {
        var command = mapper.Map<UpdateFavoriteAddressCommand>(dto);
        command.UserId = UserId;

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    [Authorize]
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
