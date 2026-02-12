using Hooome.Application.Interfaces;
using MediatR;
using Serilog;

namespace Hooome.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ICurrentUserService currentUserService)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = currentUserService.UserId;

        Log.Information("Hooome Request: {Name} {@UserId} {@Request}", requestName, userId, request);

        return await next(cancellationToken);
    }
}
