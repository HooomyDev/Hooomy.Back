using AutoMapper;
using Hooome.Application.CQRS.Works.Queries.GetWorkList;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Works.Queries.GetWorkListForEmployee;

public class GetWorkListForEmployeeQueryHandler(IWorkRepository workRepo, IMapper mapper)
    : IRequestHandler<GetWorkListForEmployeeQuery, GetWorkListWithPaginationVm>
{
    public async Task<GetWorkListWithPaginationVm> Handle(GetWorkListForEmployeeQuery request, CancellationToken cancellationToken)
    {
        var (works, totalCount) = await workRepo.GetAllWithPagination(
            page: request.Page,
            pageSize: request.PageSize,
            category: request.Category,
            seriousness: request.Seriousness,
            addressId: request.AddressId,
            searchTitle: request.SearchTitle,
            companyId: request.CompanyId,
            cancellationToken: cancellationToken);

        var workDtos = mapper.Map<List<WorkListLookupDto>>(works);

        return new GetWorkListWithPaginationVm
        {
            Works = workDtos,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}
