using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.UpdatePoll;

public class UpdatePollCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<UpdatePollCommand>
{
    public async Task Handle(UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var poll = await dbContext.Polls
            .FindAsync([request.Id], cancellationToken);

        if (poll is null || poll.CreatedBy != request.CreatedBy)
        {
            throw new NotFoundException(nameof(Poll), request.Id);
        }

        poll.Title = request.Title;
        poll.Description = request.Description;
        poll.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}