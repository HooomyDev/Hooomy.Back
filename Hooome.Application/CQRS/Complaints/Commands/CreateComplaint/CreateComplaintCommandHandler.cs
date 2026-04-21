using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Commands.CreateComplaint;

public class CreateComplaintCommandHandler(IComplaintRepository complaintRepo)
    : IRequestHandler<CreateComplaintCommand, Guid>
{
    public async Task<Guid> Handle(CreateComplaintCommand request, CancellationToken cancellationToken)
    {
        var newComplaint = new Complaint
        {
            Id = Guid.NewGuid(),
            ShortDescription = request.ShortDescription,
            Description = request.Description,
            Type = request.Type,
            Status = ComplaintStatus.AcceptedForReview,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
        };

        await complaintRepo.Create(newComplaint, cancellationToken);

        return newComplaint.Id;
    }
}