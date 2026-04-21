using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Commands.CreateComplaint;

public class CreateComplaintCommand : IRequest<Guid>
{
    public string ShortDescription { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ComplaintType Type { get; set; } = ComplaintType.Unknown;
}
