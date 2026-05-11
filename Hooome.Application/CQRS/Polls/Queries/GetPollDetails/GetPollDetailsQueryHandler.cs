using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollDetails;

public class GetPollDetailsQueryHandler(IPollRepository pollRepo, IMapper mapper)
    : IRequestHandler<GetPollDetailsQuery, PollDetailsVm>
{
    public async Task<PollDetailsVm> Handle(GetPollDetailsQuery request, CancellationToken cancellationToken)
    {
        var poll = await pollRepo.GetById(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Poll), request.Id);

        var pollVm = mapper.Map<PollDetailsVm>(poll);

        pollVm.UserVotes = [.. poll.Votes
            .Where(v => v.UserId == request.UserId)
            .Select(v => v.OptionId)];

        return pollVm;
    }
}
