using MediatR;

namespace Hooome.Application.CQRS.Complaints.Commands.DeleteComplaint;

public class DeleteComplaintCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}
