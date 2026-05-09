namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;

public class RequestDailyStatisticVm
{
    public IList<RequestDailyStatisticLookupDto> RequestsByDates { get; set; } = [];
    public IList<RequestByStatusesLookupDto> RequestsByStatuses { get; set; } = [];
    public IList<RequestByCategoriesLookupDto> RequestsByCategories { get; set; } = [];
    public int TotalCount { get; set; }
}
