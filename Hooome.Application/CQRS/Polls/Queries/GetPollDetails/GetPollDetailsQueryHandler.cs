using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollDetails;

public class GetPollDetailsQueryHandler(IHooomeDbContext dbContext)
    : IRequestHandler<GetPollDetailsQuery, PollDetailsVm>
{
    public async Task<PollDetailsVm> Handle(GetPollDetailsQuery request, CancellationToken cancellationToken)
    {
        var poll = await dbContext.Polls
            .Include(p => p.Votes)
            .Include(p => p.Company)
            .Include(p => p.Options)
                .ThenInclude(o => o.Votes)
            .Where(p => p.Id == request.Id)
            .Select(p => new PollDetailsVm
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CompanyName = p.Company.Name,
                VoteCount = p.Votes.Count,
                IsActive = p.IsActive,
                Type = p.Type,
                Options = p.Options.Select(o => new PollOptionLookupDto
                {
                    Id = o.Id,
                    VoteCount = o.Votes.Count,
                    Content = o.Content,
                })
                .OrderByDescending(o => o.VoteCount)
                .ToList(),
                UserVotes = p.Votes
                    .Where(v => v.UserId == request.UserId)
                    .Select(v => v.OptionId)
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException(nameof(Poll), request.Id);

        return poll;
    }
}
