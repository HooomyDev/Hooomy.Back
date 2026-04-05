namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;

public class RequestDailyStatisticLookupDto
{
    public string Date { get; set; } = string.Empty;
    public int Count { get; set; }
    public string DisplayDate { get; set; } = string.Empty;
    public bool IsToday { get; set; }
    public bool HasData { get; set; }
}
