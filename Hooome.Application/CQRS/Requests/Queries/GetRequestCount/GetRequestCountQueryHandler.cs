using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCount;

public class GetRequestCountQueryHandler(IRequestRepository requestRepo)
    : IRequestHandler<GetRequestCountQuery, int>
{
    public async Task<int> Handle(GetRequestCountQuery request, CancellationToken cancellationToken)
    {
        var requestCount = await requestRepo.Count(request.Status, cancellationToken);

        return requestCount;
    }
}
