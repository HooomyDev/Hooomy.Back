using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.Complaints.Commands.UpdateComplaint;

public class UpdateComplaintCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<UpdateComplaintCommand>
{
    public async Task Handle(UpdateComplaintCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Complaints
            .FindAsync([request.Id], cancellationToken);

        if (entity == null || entity.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(Request), request.Id);
        }

        entity.ShortDescription = request.ShortDescription;
        entity.Description = request.Description;
        entity.Status = request.Status;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}