using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hooome.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class BaseController : ControllerBase
{
    private IMediator _mediator = null!;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
        ?? throw new NullReferenceException("Mediator is null.");

    protected Guid UserId
    {
        get
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return Guid.Empty;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub") 
                ?? User.FindFirst("uid"); 

            if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
                return Guid.Empty;

            return Guid.TryParse(userIdClaim.Value, out var userId)
                ? userId
                : Guid.Empty;
        }
    }

    protected bool IsInRole(string role) =>
        User.IsInRole(role);

    protected ObjectResult ApiResult<T>(T data, string? message = null)
    {
        return Ok(new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Timestamp = DateTime.UtcNow
        });
    }

    protected ObjectResult ErrorResult(string message,
        Dictionary<string, string[]>? errors = null,
        int statusCode = StatusCodes.Status400BadRequest)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Errors = errors,
            Timestamp = DateTime.UtcNow
        };

        return StatusCode(statusCode, response);
    }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
    public DateTime Timestamp { get; set; }
}