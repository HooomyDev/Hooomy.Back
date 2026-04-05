using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCount;

public class GetRequestCountQueryHandler(IHooomeDbContext dbContext)
    : IRequestHandler<GetRequestCountQuery, int>
{
    public async Task<int> Handle(GetRequestCountQuery request, CancellationToken cancellationToken)
    {
        var requestCount = await dbContext.Requests.CountAsync(cancellationToken);

        return requestCount;
    }
}
