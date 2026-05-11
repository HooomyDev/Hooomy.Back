using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollList;

public class PollListVm
{
    public IList<PollListLookupDto> Polls { get; set; } = [];
    public int Page { get; set; }           
    public int PageSize { get; set; }       
    public int TotalCount { get; set; }     
    public int TotalPages { get; set; }
    public PollStatus Status { get; set; }
    public PollType Type { get; set; }
}
