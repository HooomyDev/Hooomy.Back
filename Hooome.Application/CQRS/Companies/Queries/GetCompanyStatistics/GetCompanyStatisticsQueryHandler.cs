using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyStatistics;

public class GetCompanyStatisticsQueryHandler(ICompanyRepository companyRepo,
    IRequestRepository requestRepo, IRequestReviewRepository requestReviewRepository)
    : IRequestHandler<GetCompanyStatisticsQuery, CompanyStatisticsVm>
{
    public async Task<CompanyStatisticsVm> Handle(GetCompanyStatisticsQuery request, CancellationToken cancellationToken)
    {
        var companies = await companyRepo.GetAll(cancellationToken);

        var statistics = await BuildStatistic([.. companies], cancellationToken);

        return new CompanyStatisticsVm
        {
            Companies = statistics
        };
    }

    private async Task<List<CompanyStatisticsLookupDto>> BuildStatistic(ICollection<Company> companies,
        CancellationToken cancellationToken)
    {
        var result = new List<CompanyStatisticsLookupDto>();
        foreach (var company in companies)
        {
            var requests = await requestRepo.GetByCompanyId(company.Id, cancellationToken);

            var totalRequestCount = requests.Count();
            var completedRequestCount = requests.Count(r => r.Status == RequestStatus.Completed);
            var pendingRequestCount = totalRequestCount - completedRequestCount;

            var reviews = await requestReviewRepository.GetByCompanyId(company.Id, cancellationToken);

            var rating = reviews?.Count > 0 ? Math.Round(reviews.Average(r => r.Score), 1) : 0;
            var ratingCount = reviews?.Count ?? 0;

            var companyDto = new CompanyStatisticsLookupDto
            {
                CompanyId = company.Id,
                CompanyName = company.Name,
                TotalRequestCount = totalRequestCount,
                CompletedRequestCount = completedRequestCount,
                PendingRequestCount = pendingRequestCount,
                Rating = rating,
                RatingCount = ratingCount
            };

            result.Add(companyDto);
        }

        return result;
    }
}
