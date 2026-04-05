namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCategoryList;

public class RequestCategoryListVm
{
    public IList<RequestCategoryListLookupDto> Categories { get; set; } = []; 
}
