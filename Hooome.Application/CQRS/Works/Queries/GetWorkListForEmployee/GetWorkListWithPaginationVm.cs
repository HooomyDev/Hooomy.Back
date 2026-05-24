using Hooome.Application.CQRS.Works.Queries.GetWorkList;

namespace Hooome.Application.CQRS.Works.Queries.GetWorkListForEmployee;

public class GetWorkListWithPaginationVm
{
    public IList<WorkListLookupDto> Works { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
