namespace Hooome.Application.CQRS.Works.Queries.GetWorkList;

public class GetWorkListVm
{
    public IList<WorkListLookupDto> Works { get; set; } = [];
}
