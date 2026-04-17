using AutoMapper;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestListWithPagination;

public class GetRequestListWithPaginationQueryHandler(IRequestRepository requestRepo, IMapper mapper)
    : IRequestHandler<GetRequestListWithPaginationQuery, RequestListWithPaginationVm>
{
    public async Task<RequestListWithPaginationVm> Handle(GetRequestListWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var (requests, totalCount) = await requestRepo.GetRequestsWithPagination(
            title: request.Title,
            status: request.Status,
            category: request.Category,
            page: request.Page,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var requestsDtos = mapper.Map<List<RequestListLookupDto>>(requests);

        return new RequestListWithPaginationVm
        {
            Requests = requestsDtos,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}