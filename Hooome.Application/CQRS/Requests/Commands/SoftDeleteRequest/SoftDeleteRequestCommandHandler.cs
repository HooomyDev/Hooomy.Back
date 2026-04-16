using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.SoftDeleteRequest;

public class SoftDeleteRequestCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<SoftDeleteRequestCommand>
{
    public async Task Handle(SoftDeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Requests
            .FindAsync([request.Id], cancellationToken);

        if (entity is null || entity.UserID != request.UserId)
        {
            return;
        }

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
