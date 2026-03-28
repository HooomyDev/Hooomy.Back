using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollList;

public class GetPollListQueryHandler(IHooomeDbContext dbContext)
    : IRequestHandler<GetPollListQuery, PollListVm>
{
    public async Task<PollListVm> Handle(GetPollListQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Polls
            .Include(p => p.Company)
            .Include(p => p.Options)
            .Include(p => p.Votes)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        if(request.FilterOption == "active")
        {
            query = query.Where(x => x.IsActive);
        }

        var polls = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new PollListLookupDto()
            {
                Id = x.Id,
                Title = x.Title,
                CompanyName = x.Company.Name,
                VoteCount = x.Votes.Count,
                IsActive = x.IsActive,
                Type = x.Type,
            })
            .ToListAsync(cancellationToken);

        return new PollListVm 
        { 
            Polls = polls,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            FilterOption = request.FilterOption,
        };
    }
}
