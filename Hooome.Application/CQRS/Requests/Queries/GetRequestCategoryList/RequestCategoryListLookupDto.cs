namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCategoryList;

public class RequestCategoryListLookupDto
{
    public int Code { get; set; }
    public string Name { get; set; } = null!;
}
