using FluentValidation;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;

public class GetRequestDailyStatisticQueryHandler(IHooomeDbContext dbContext)
    : IRequestHandler<GetRequestDailyStatisticQuery, RequestDailyStatisticVm>
{
    public async Task<RequestDailyStatisticVm> Handle(
        GetRequestDailyStatisticQuery request,
        CancellationToken cancellationToken)
    {
        var (startDate, endDate) = GetDateRange(request.Period);
        var now = DateTime.Now.Date;

        var requests = await dbContext.Requests
            .Where(x => x.CreatedAt.Date >= startDate && x.CreatedAt.Date <= endDate)
            .ToListAsync(cancellationToken);

        var groupByMonth = request.Period == RequestsPeriod.HalfYear ||
                          request.Period == RequestsPeriod.Year;

        List<RequestDailyStatisticLookupDto> statistics;

        if (groupByMonth)
        {
            // Группировка существующих запросов по месяцам
            var groupedData = requests
                .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
                .ToDictionary(
                    g => $"{g.Key.Year}-{g.Key.Month:D2}",
                    g => g.Count()
                );

            // Создаем список всех месяцев в диапазоне
            statistics = new List<RequestDailyStatisticLookupDto>();
            var currentMonth = new DateTime(startDate.Year, startDate.Month, 1);
            var lastMonth = new DateTime(endDate.Year, endDate.Month, 1);

            while (currentMonth <= lastMonth)
            {
                var monthKey = currentMonth.ToString("yyyy-MM");
                var count = groupedData.GetValueOrDefault(monthKey, 0);

                statistics.Add(new RequestDailyStatisticLookupDto
                {
                    Date = monthKey,
                    Count = count,
                    DisplayDate = currentMonth.ToString("MMM yyyy"),
                    IsToday = false,
                    HasData = count > 0
                });

                currentMonth = currentMonth.AddMonths(1);
            }
        }
        else
        {
            // Группировка существующих запросов по дням
            var groupedData = requests
                .GroupBy(r => r.CreatedAt.Date)
                .ToDictionary(
                    g => g.Key.ToString("yyyy-MM-dd"),
                    g => g.Count()
                );

            // Создаем список всех дней в диапазоне
            statistics = new List<RequestDailyStatisticLookupDto>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var dateKey = currentDate.ToString("yyyy-MM-dd");
                var count = groupedData.GetValueOrDefault(dateKey, 0);

                statistics.Add(new RequestDailyStatisticLookupDto
                {
                    Date = dateKey,
                    Count = count,
                    DisplayDate = FormatDisplayDate(currentDate),
                    IsToday = currentDate == now,
                    HasData = count > 0
                });

                currentDate = currentDate.AddDays(1);
            }
        }

        return new RequestDailyStatisticVm { Requests = statistics };
    }

    private static (DateTime startDate, DateTime endDate) GetDateRange(RequestsPeriod period)
    {
        var now = DateTime.Now.Date;

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