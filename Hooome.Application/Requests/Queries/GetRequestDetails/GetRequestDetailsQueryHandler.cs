using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.Requests.Queries.GetRequestDetails;

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

        return mapper.Map<RequestDetailsVm>(entity);
    }
}
