namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCateryList;

public class RequestCategoryListVm
{
    public IList<RequestCategoryListLookupDto> Categories { get; set; } = []; 
}
