namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyStatistics;

public class CompanyStatisticsLookupDto 
{
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; } = null!;
    
    public int TotalRequestCount { get; set; }
    public int CompletedRequestCount { get; set; }
    public int PendingRequestCount { get; set; }
    
    public double Rating { get; set; } 
    public int RatingCount { get; set; }
}
