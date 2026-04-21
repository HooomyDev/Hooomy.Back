using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Commands.UpdateComplaint;

public class UpdateComplaintCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<UpdateComplaintCommand>
{
    public async Task Handle(UpdateComplaintCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Complaints
            .FindAsync([request.Id], cancellationToken) 
            ?? throw new NotFoundException(nameof(Request), request.Id);

        if (!string.IsNullOrEmpty(request.ShortDescription))
            entity.ShortDescription = request.ShortDescription;

        if (!string.IsNullOrEmpty(request.Description))
            entity.Description = request.Description;

        entity.Status = request.Status;
        entity.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}