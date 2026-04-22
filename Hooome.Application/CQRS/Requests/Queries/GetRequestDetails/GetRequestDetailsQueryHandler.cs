using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDetails;

public class GetRequestDetailsQueryHandler(IRequestRepository requestRepo, IMapper mapper, IMinioService minioService)
    : IRequestHandler<GetRequestDetailsQuery, RequestDetailsVm>
{
    public async Task<RequestDetailsVm> Handle(GetRequestDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await requestRepo
            .GetByIdAndUserId(request.Id, request.UserId, cancellationToken) 
            ?? throw new NotFoundException(nameof(Request), request.Id);

        var requestDetails = mapper.Map<RequestDetailsVm>(entity);

        var imageUrls = new List<string>();
        foreach (var imageName in requestDetails.ImagesUrls)
        {
            var url = minioService.GetUrl(ImageType.Request, imageName);

            imageUrls.Add(url);
        }

        requestDetails.ImagesUrls = imageUrls;


        return requestDetails;
    }
}
