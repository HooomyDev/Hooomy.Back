using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.Complaints.Queries.GetComplaintList;

public class GetComplaintListQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetComplaintListQuery, ComplaintListVm>
{
    public async Task<ComplaintListVm> Handle(GetComplaintListQuery request, CancellationToken cancellationToken)
    {
        var complaints = await dbContext.Complaints
            .Where(x => x.UserId == request.UserId)
            .ProjectTo<ComplaintListLookupDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ComplaintListVm { Complaints = complaints };
    }
}
