using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.UpdatePoll;

public class UpdatePollCommandHandler(IPollRepository pollRepo, IMapper mapper)
    : IRequestHandler<UpdatePollCommand>
{
    public async Task Handle(UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepo.GetById(request.Id, cancellationToken);

        if (poll is null || poll.CreatedBy != request.CreatedBy)
        {
            throw new NotFoundException(nameof(Poll), request.Id);
        }

        mapper.Map(request, poll);
        poll.UpdatedAt = DateTime.UtcNow;

        await pollRepo.SaveChanges(cancellationToken);
    }
}