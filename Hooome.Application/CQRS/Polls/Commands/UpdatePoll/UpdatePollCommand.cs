using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.UpdatePoll;

public class UpdatePollCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsActive { get; set; }
}
