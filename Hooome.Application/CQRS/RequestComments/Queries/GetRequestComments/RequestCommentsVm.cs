namespace Hooome.Application.CQRS.RequestComments.Queries.GetRequestComments;

public class RequestCommentsVm
{
    public IList<RequestCommentLookupDto> RequestComments { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
