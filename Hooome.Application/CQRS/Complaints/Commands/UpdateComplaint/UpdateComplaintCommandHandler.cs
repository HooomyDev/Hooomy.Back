using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Commands.UpdateComplaint;

public class UpdateComplaintCommandHandler(IComplaintRepository complaintRepo)
    : IRequestHandler<UpdateComplaintCommand>
{
    public async Task Handle(UpdateComplaintCommand request, CancellationToken cancellationToken)
    {
        var entity = await complaintRepo.GetById(request.Id, cancellationToken)
         ?? throw new NotFoundException(nameof(Complaint), request.Id);

        if (!string.IsNullOrWhiteSpace(request.ShortDescription))
            entity.ShortDescription = request.ShortDescription.Trim();

        if (!string.IsNullOrWhiteSpace(request.Description))
            entity.Description = request.Description.Trim();

        entity.Status = request.Status;
        entity.UpdatedAt = DateTime.UtcNow;

        await complaintRepo.Update(entity, cancellationToken);
    }
}