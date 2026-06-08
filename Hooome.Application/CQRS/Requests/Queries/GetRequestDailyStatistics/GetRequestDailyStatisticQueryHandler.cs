using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;

public class GetRequestDailyStatisticQueryHandler(IRequestRepository requestRepo)
    : IRequestHandler<GetRequestDailyStatisticQuery, RequestDailyStatisticVm>
{
    public async Task<RequestDailyStatisticVm> Handle(
            GetRequestDailyStatisticQuery request,
            CancellationToken cancellationToken)
    {
        var (startDate, endDate) = GetDateRange(request.Period);
        var now = DateTime.UtcNow.Date;

        var groupedData = await requestRepo.GetRequestsByDate(
            startDate, endDate, request.CompanyId, cancellationToken);

        var requestsByDates = BuildDailyStatistics(startDate, endDate, groupedData, now);

        var (requests, totalCount) = await requestRepo.GetRequestsWithPagination(
            page: 1,
            pageSize: 1000,
            companyId: request.CompanyId,
            cancellationToken: cancellationToken);

        var requestsByStatuses = BuildStatisticByStatuses([.. requests]);

        var requestsByCategories = BuildStatisticByCategories([.. requests]);

        return new RequestDailyStatisticVm 
        { 
            RequestsByDates = requestsByDates, 
            RequestsByStatuses = requestsByStatuses,
            RequestsByCategories = requestsByCategories,
            TotalCount = totalCount,
        };
    }

    private static List<RequestDailyStatisticLookupDto> BuildDailyStatistics(
        DateTime startDate,
        DateTime endDate,
        Dictionary<string, int> groupedData,
        DateTime now)
    {
        var statistics = new List<RequestDailyStatisticLookupDto>();

        var current = startDate;

        while (current <= endDate)
        {
            var key = current.ToString("yyyy-MM-dd");
            var count = groupedData.GetValueOrDefault(key, 0);

            statistics.Add(new RequestDailyStatisticLookupDto
            {
                Date = key,
                Count = count,
                DisplayDate = current.ToString("ddd, MMM d"),
                IsToday = current == now,
                HasData = count > 0
            });

            current = current.AddDays(1);
        }

        return statistics;
    }

    private static (DateTime startDate, DateTime endDate) GetDateRange(Period period)
    {
        var now = DateTime.UtcNow.Date;

        return period switch
        {
            Period.Week => (now.AddDays(-6), now),
            Period.TwoWeek => (now.AddDays(-13), now),
            Period.Month => (now.AddDays(-29), now),
            _ => (now.AddDays(-6), now)
        };
    }

    private static List<RequestByStatusesLookupDto> BuildStatisticByStatuses(List<Request> requests)
    {
        var statistics = requests
            .GroupBy(r => r.Status)
            .Select(g => new RequestByStatusesLookupDto
            {
                Status = g.Key,
                Count = g.Count(),
                Percentage = Math.Round((double)g.Count() / requests.Count * 100, 2)
            })
            .OrderBy(r => r.Status)
            .ToList();

        return statistics;
    }

    private static List<RequestByCategoriesLookupDto> BuildStatisticByCategories(List<Request> requests)
    {
        var statistics = requests
            .GroupBy(r => r.Category)
            .Select(g => new RequestByCategoriesLookupDto
            {
                Category = g.Key,
                Count = g.Count(),
                Percentage = Math.Round((double)g.Count() / requests.Count * 100, 2)
            })
            .OrderBy(r => r.Category)
            .ToList();

        return statistics;
    }
}
