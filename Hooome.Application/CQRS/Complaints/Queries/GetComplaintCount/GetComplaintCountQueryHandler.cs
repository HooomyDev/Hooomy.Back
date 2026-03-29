using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintCount;

public class GetComplaintCountQueryHandler(IHooomeDbContext dbContext)
    : IRequestHandler<GetComplaintCountQuery, int>
{
    public async Task<int> Handle(GetComplaintCountQuery request, CancellationToken cancellationToken)
    {
        var count = await dbContext.Complaints.CountAsync(cancellationToken);

        return count;
    }
}