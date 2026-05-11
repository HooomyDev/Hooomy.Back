using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.CreatePoll;

public class CreatePollCommand : IRequest<Guid>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid CreatedBy { get; set; }
    public Guid CompanyId { get; set; }
    public PollType Type { get; set; }
    public IList<PollOptionDto> Options { get; set; } = [];
}
