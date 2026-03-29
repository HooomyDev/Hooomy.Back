using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Commands.DeleteComplaint;

public class DeleteComplaintCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<DeleteComplaintCommand>
{
    public async Task Handle(DeleteComplaintCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Complaints
            .FindAsync([request.Id], cancellationToken);

        if (entity is null || entity.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(Complaints), request.Id);
        }

        dbContext.Complaints.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
