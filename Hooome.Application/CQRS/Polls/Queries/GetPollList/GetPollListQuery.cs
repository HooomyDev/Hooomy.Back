using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollList;

public class GetPollListQuery : IRequest<PollListVm>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string Title { get; set; } = string.Empty;
    public PollStatus Status { get; set; }
    public PollType Type { get; set; }
    public Guid? CompanyId { get; set; }
}
