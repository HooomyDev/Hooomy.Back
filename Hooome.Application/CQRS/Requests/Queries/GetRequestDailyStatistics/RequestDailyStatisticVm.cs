namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;

public class RequestDailyStatisticVm
{
    public IList<RequestDailyStatisticLookupDto> Requests { get; set; } = [];
}
