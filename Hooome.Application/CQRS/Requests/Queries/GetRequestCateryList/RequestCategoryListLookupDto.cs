namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCateryList;

public class RequestCategoryListLookupDto
{
    public int Code { get; set; }
    public string Name { get; set; } = null!;
}
