using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintList;

public class GetComplaintListQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetComplaintListQuery, ComplaintListVm>
{
    public async Task<ComplaintListVm> Handle(GetComplaintListQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Complaints.AsQueryable();

        if (request.Status != Domain.Enums.ComplaintStatus.Unknown)
        {
            query = query.Where(c => c.Status == request.Status);
        }

        if (request.Type != Domain.Enums.ComplaintType.Unknown)
        {
            query = query.Where(c => c.Type == request.Type);
        }

        if (!string.IsNullOrWhiteSpace(request.ShortDescription))
        {
            query = query.Where(c => c.ShortDescription.Contains(request.ShortDescription));
        }

        var complaints = await query
            .ProjectTo<ComplaintListLookupDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ComplaintListVm { Complaints = complaints };
    }
}