namespace Hooome.Application.CQRS.Requests.Queries.GetRequestList;

public class RequestListVm
{
    public IList<RequestListDto> Requests { get; set; } = [];
}