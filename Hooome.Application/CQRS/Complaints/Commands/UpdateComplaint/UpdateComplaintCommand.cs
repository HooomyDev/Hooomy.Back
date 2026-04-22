using Hooome.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Complaints.Commands.UpdateComplaint;

public class UpdateComplaintCommand : IRequest
{
    public Guid Id { get; set; }
    public string ShortDescription { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ComplaintStatus Status { get; set; } = ComplaintStatus.Unknown;
}
