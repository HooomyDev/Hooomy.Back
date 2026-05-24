namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyStatistics;

public class CompanyStatisticsVm
{
    public IList<CompanyStatisticsLookupDto> Companies { get; set; } = [];
}
