using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Commands.CreateComplaint;

public class CreateComplaintCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string ShortDescription { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ComplaintType Type { get; set; } = ComplaintType.Unknown;
    public Guid? RequestId { get; set; }
}
