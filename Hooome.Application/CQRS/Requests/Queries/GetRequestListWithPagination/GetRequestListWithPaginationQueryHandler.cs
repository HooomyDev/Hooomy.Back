using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestListWithPagination;

public class GetRequestListWithPaginationQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetRequestListWithPaginationQuery, RequestListWithPaginationVm>
{
    public async Task<RequestListWithPaginationVm> Handle(GetRequestListWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Requests
            .Include(r => r.Address)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = query.Where(r => r.Title.Contains(request.Title));
        }

        if (request.Status != Domain.Enums.RequestStatus.Unknown)
        {
            query = query.Where(r => r.Status == request.Status);
        }

        if (request.Category != Domain.Enums.RequestCategory.None)
        {
            query = query.Where(r => r.Category == request.Category);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var requests = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<RequestListLookupDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new RequestListWithPaginationVm
        {
            Requests = requests,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}