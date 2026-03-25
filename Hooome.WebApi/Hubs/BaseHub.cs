using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Hooome.WebApi.Hubs;

public abstract class BaseHub<T>: Hub<T> 
    where T : class
{
    private IMediator _mediator = null!;

    protected IMediator Mediator => 
        _mediator ??= Context.GetHttpContext()?
        .RequestServices.GetService<IMediator>()
        ?? throw new NullReferenceException("Mediator is null.");

    public Guid UserId
    {
        get
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)
                ?? Context.User?.FindFirst("sub")
                ?? Context.User?.FindFirst("uid");

            if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
                return Guid.Empty;

            return Guid.TryParse(userIdClaim.Value, out var userId)
                ? userId
                : Guid.Empty;
        }
    }
}
