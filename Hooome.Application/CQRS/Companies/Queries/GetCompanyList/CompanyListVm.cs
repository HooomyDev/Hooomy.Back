namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyList;

public class CompanyListVm
{
    public IList<CompanyListLookupDto> Companies { get; set; } = [];
}
