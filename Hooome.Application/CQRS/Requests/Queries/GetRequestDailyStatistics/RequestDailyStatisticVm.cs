namespace Hooome.Application.Requests.Queries.GetRequestDailyStatistics;

public class RequestDailyStatisticVm
{
    public IList<RequestDailyStatisticLookupDto> Requests { get; set; } = [];
}
