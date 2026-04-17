using Hooome.Application.Interfaces;
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
        
        var groupType = request.Period == RequestsPeriod.HalfYear || request.Period == RequestsPeriod.Year 
            ? StatisticGroupType.Month 
            : StatisticGroupType.Day;

        var groupedData = await requestRepo.GetRequestsCount(
            startDate, endDate, groupType, cancellationToken);

        var statistics = BuildStatistics(startDate, endDate, groupedData, groupType, now);

        return new RequestDailyStatisticVm { Requests = statistics };
    }

    private static List<RequestDailyStatisticLookupDto> BuildStatistics(
        DateTime startDate,
        DateTime endDate,
        Dictionary<string, int> groupedData,
        StatisticGroupType groupType,
        DateTime now)
    {
        var statistics = new List<RequestDailyStatisticLookupDto>();

        if (groupType == StatisticGroupType.Month)
        {
            var current = new DateTime(startDate.Year, startDate.Month, 1);
            var last = new DateTime(endDate.Year, endDate.Month, 1);

            while (current <= last)
            {
                var key = current.ToString("yyyy-MM");
                var count = groupedData.GetValueOrDefault(key, 0);

                statistics.Add(new RequestDailyStatisticLookupDto
                {
                    Date = key,
                    Count = count,
                    DisplayDate = current.ToString("MMM yyyy"),
                    IsToday = false,
                    HasData = count > 0
                });

                current = current.AddMonths(1);
            }
        }
        else
        {
            var current = startDate;

            while (current <= endDate)
            {
                var key = current.ToString("yyyy-MM-dd");
                var count = groupedData.GetValueOrDefault(key, 0);

                statistics.Add(new RequestDailyStatisticLookupDto
                {
                    Date = key,
                    Count = count,
                    DisplayDate = FormatDisplayDate(current),
                    IsToday = current == now,
                    HasData = count > 0
                });

                current = current.AddDays(1);
            }
        }

        return statistics;
    }

    private static (DateTime startDate, DateTime endDate) GetDateRange(RequestsPeriod period)
    {
        var now = DateTime.UtcNow.Date;

        return period switch
        {
            RequestsPeriod.Week => (now.AddDays(-6), now),
            RequestsPeriod.TwoWeek => (now.AddDays(-13), now),
            RequestsPeriod.Month => (now.AddDays(-29), now),
            RequestsPeriod.CurrentMonth => (new DateTime(now.Year, now.Month, 1), now),
            RequestsPeriod.HalfYear => (now.AddMonths(-5).AddDays(1 - now.Day), now),
            RequestsPeriod.Year => (now.AddMonths(-11).AddDays(1 - now.Day), now),
            _ => (now.AddDays(-6), now)
        };
    }

    private static string FormatDisplayDate(DateTime date)
    {
        return date.ToString("ddd, MMM d");
    }
}