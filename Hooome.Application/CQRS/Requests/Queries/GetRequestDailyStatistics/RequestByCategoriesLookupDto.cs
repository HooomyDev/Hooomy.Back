using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;

public class RequestByCategoriesLookupDto
{
    public RequestCategory Category { get; set; }
    public int Count { get; set; }
    public double Percentage { get; set; }
}