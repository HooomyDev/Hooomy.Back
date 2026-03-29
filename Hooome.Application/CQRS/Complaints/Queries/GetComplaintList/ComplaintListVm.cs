namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintList;

public class ComplaintListVm
{
    public IList<ComplaintListLookupDto> Complaints { get; set; } = [];
}
