using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Commands.CreateComplaint;

public class CreateComplaintCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<CreateComplaintCommand, Guid>
{
    public async Task<Guid> Handle(CreateComplaintCommand request, CancellationToken cancellationToken)
    {
        var newComplaint = new Complaint
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ShortDescription = request.ShortDescription,
            Description = request.Description,
            Type = request.Type,
            Status = ComplaintStatus.AcceptedForReview,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            RequestId = request.Type == ComplaintType.Request ? request.RequestId : null
        };

        await dbContext.Complaints.AddAsync(newComplaint, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newComplaint.Id;
    }
}