using MediatR;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintDetails;

public class GetComplaintDetailsQuery : IRequest<ComplaintDetailsVm>
{
    public Guid Id { get; set; }
}
