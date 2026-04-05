using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestList;

public class GetRequestsListQueryHandler(IHooomeDbContext dbContext, IMapper mapper) 
    : IRequestHandler<GetRequestListQuery, RequestListVm>
{
    public async Task<RequestListVm> Handle(GetRequestListQuery request, CancellationToken cancellationToken)
    {
        var requests = await dbContext.Requests
            .Where(r => r.UserID == request.UserId)
            .ProjectTo<RequestListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new RequestListVm { Requests = requests };
    }
}