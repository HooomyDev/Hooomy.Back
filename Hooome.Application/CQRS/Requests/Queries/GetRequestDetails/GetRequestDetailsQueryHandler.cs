using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDetails;

public class GetRequestDetailsQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetRequestDetailsQuery, RequestDetailsVm>
{
    public async Task<RequestDetailsVm> Handle(GetRequestDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.Requests
            .FindAsync([request.Id], cancellationToken);

        if (entity is null || entity.UserID != request.UserId)
        {
            throw new NotFoundException(nameof(Request), request.Id);
        }

        var requestDetails = mapper.Map<RequestDetailsVm>(entity);

        requestDetails.ImagesUrls = await dbContext.Images
            .Where(i => i.RequestId == request.Id)
            .Select(i => $"{i.FilePath}")
            .ToListAsync(cancellationToken);

        requestDetails.Address = await dbContext.Addresses
            .Where(a => a.Id == entity.AddressId)
            .Select(a => $"{a.Street}, {a.HouseNumber}")
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Address), entity.AddressId);

        return requestDetails;
    }
}
