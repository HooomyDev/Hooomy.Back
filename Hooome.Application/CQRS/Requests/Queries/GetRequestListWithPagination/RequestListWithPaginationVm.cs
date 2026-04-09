namespace Hooome.Application.CQRS.Requests.Queries.GetRequestListWithPagination;

public class RequestListWithPaginationVm
{
    public IList<RequestListLookupDto> Requests { get; set; } = [];
    public int Page {  get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
