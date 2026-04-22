using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintList;

public class GetComplaintListQuery : IRequest<ComplaintListVm>
{
    public ComplaintStatus Status { get; set; }
    public ComplaintType Type { get; set; }
    public string? ShortDescription { get; set; }
}
