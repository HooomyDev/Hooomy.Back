using AutoMapper;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestList;

public class GetRequestsListQueryHandler(IRequestRepository requestRepo, IMapper mapper) 
    : IRequestHandler<GetRequestListQuery, RequestListVm>
{
    public async Task<RequestListVm> Handle(GetRequestListQuery request, CancellationToken cancellationToken)
    {
        var requests = await requestRepo.GetFilteredRequests(
            userId: request.UserId,
            category: request.RequestCategory,
            addressId: request.AddressId,
            status: request.RequestStatus,
            searchTitle: request.SearchTitle,
            cancellationToken: cancellationToken);

        var requestsDtos = mapper.Map<List<RequestListDto>>(requests);

        return new RequestListVm { Requests = requestsDtos };
    }
}