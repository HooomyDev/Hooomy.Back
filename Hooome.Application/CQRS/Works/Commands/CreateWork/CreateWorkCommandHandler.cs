using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Works.Commands.CreateWork;

public class CreateWorkCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<CreateWorkCommand, Guid>
{
    public async Task<Guid> Handle(CreateWorkCommand request, CancellationToken cancellationToken)
    {
        var newWork = new Work
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            AddressId = request.AddressId,
            Seriousness = request.Seriousness,
            PlannedStartTime = request.PlannedStartTime,
            PlannedEndTime = request.PlannedEndTime,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        await dbContext.Works.AddAsync(newWork, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newWork.Id;
    }
}
