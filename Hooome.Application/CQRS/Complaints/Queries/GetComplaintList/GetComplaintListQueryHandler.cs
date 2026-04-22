using AutoMapper;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintList;

public class GetComplaintListQueryHandler(IComplaintRepository complaintRepo, IMapper mapper)
    : IRequestHandler<GetComplaintListQuery, ComplaintListVm>
{
    public async Task<ComplaintListVm> Handle(GetComplaintListQuery request, CancellationToken cancellationToken)
    {
        var complaints = await complaintRepo.GetAllWithFilters(
            request.Status,
            request.Type,
            request.ShortDescription ?? "",
            cancellationToken);

        var complaintDtos = mapper.Map<List<ComplaintListLookupDto>>(complaints);

        return new ComplaintListVm { Complaints = complaintDtos };
    }
}