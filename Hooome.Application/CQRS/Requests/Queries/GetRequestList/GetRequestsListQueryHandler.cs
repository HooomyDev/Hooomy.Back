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
        var query = dbContext.Requests
            .Include(r => r.Address)
            .Where(r => r.UserID == request.UserId)
            .AsQueryable();

        if(request.StartDate is not null)
            query = query.Where(r => r.CreatedAt >= request.StartDate);
        
        if(request.EndDate is not null)
            query = query.Where(r => r.CreatedAt <= request.EndDate.Value.Date.AddDays(1));

        if(request.RequestStatus != Domain.Enums.RequestStatus.Unknown)
            query = query.Where(r => r.Status == request.RequestStatus);

        var requests = await query
            .OrderByDescending(r => r.CreatedAt)
            .ProjectTo<RequestListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new RequestListVm { Requests = requests };
    }
}