using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintCount;

public class GetComplaintCountQueryHandler(IComplaintRepository complaintRepo)
    : IRequestHandler<GetComplaintCountQuery, int>
{
    public async Task<int> Handle(GetComplaintCountQuery request, CancellationToken cancellationToken)
    {
        var count = await complaintRepo.Count(cancellationToken);

        return count;
    }
}