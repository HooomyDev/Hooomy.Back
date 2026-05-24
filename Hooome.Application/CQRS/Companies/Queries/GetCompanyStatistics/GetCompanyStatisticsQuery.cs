using MediatR;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyStatistics;

public class GetCompanyStatisticsQuery : IRequest<CompanyStatisticsVm>
{
}
