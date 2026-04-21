using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintDetails;

public class GetComplaintDetailsQuery : IRequest<ComplaintDetailsVm>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public class GetComplaintDetailsQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetComplaintDetailsQuery, ComplaintDetailsVm>
{
    public async Task<ComplaintDetailsVm> Handle(GetComplaintDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Complaints
            .FindAsync([request.Id], cancellationToken) 
            ?? throw new NotFoundException(nameof(Complaint), request.Id);

        return mapper.Map<ComplaintDetailsVm>(entity);
    }
}