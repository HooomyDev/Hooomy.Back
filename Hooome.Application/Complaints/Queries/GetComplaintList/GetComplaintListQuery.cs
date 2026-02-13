using MediatR;

namespace Hooome.Application.Complaints.Queries.GetComplaintList;

public class GetComplaintListQuery : IRequest<ComplaintListVm>
{
    public Guid UserId { get; set; }
}
