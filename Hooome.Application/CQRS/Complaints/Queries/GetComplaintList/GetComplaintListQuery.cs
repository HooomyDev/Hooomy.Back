using MediatR;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintList;

public class GetComplaintListQuery : IRequest<ComplaintListVm>
{
    public Guid UserId { get; set; }
}
