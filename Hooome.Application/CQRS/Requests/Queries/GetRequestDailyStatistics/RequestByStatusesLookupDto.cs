using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;

public class RequestByStatusesLookupDto 
{
    public RequestStatus Status { get; set; }
    public int Count { get; set; } = 0;
    public double Percentage { get; set; } = 0f;
}