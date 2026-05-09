using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.DeletePoll;

public class DeletePollCommandHandler(IPollRepository pollRepo)
    : IRequestHandler<DeletePollCommand>
{
    public async Task Handle(DeletePollCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepo.GetById(request.Id, cancellationToken);

        if (poll is null || poll.CreatedBy != request.CreatedBy)
        {
            throw new NotFoundException(nameof(Request), request.Id);
        }

        await pollRepo.Delete(poll, cancellationToken);
    }
}
