using Hooome.Application.Interfaces;
using System.Security.Claims;

namespace Hooome.WebApi.Services;

public class CurrentUserService(IHttpContextAccessor accessor)
    : ICurrentUserService
{
    public Guid UserId
    {
        get
        {
            var id = accessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
        }
    }
}
