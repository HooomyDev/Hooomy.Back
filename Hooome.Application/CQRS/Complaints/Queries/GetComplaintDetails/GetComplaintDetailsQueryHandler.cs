using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintDetails;

public class GetComplaintDetailsQueryHandler(IComplaintRepository complaintRepo, IMapper mapper)
    : IRequestHandler<GetComplaintDetailsQuery, ComplaintDetailsVm>
{
    public async Task<ComplaintDetailsVm> Handle(GetComplaintDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await complaintRepo.GetById(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Complaint), request.Id);

        return mapper.Map<ComplaintDetailsVm>(entity);
    }
}