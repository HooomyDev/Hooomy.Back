using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.CreatePoll;

public class CreatePollCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<CreatePollCommand, Guid>
{
    public async Task<Guid> Handle(CreatePollCommand request, CancellationToken cancellationToken)
    {
        var newPoll = new Poll()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            CreatedBy = request.CreatedBy,
            CompanyId = request.CompanyId,
            Type = request.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await dbContext.Polls.AddAsync(newPoll, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return newPoll.Id;
    }
}
