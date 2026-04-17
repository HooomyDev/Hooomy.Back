using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestList;

public class GetRequestsListQueryHandler(IRequestRepository requestRepo, IMapper mapper) 
    : IRequestHandler<GetRequestListQuery, RequestListVm>
{
    public async Task<RequestListVm> Handle(GetRequestListQuery request, CancellationToken cancellationToken)
    {
        var requests = await requestRepo.GetFilteredRequests(
            userId: request.UserId,
            startDate: request.StartDate,
            endDate: request.EndDate,
            status: request.RequestStatus,
            cancellationToken: cancellationToken);

        var requestsDtos = mapper.Map<List<RequestListDto>>(requests);

        return new RequestListVm { Requests = requestsDtos };
    }
}