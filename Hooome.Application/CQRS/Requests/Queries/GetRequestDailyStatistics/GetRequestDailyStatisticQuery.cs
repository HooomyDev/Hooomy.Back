using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;

public class GetRequestDailyStatisticQuery : IRequest<RequestDailyStatisticVm>
{
    public RequestsPeriod Period { get; set; } = RequestsPeriod.Week;
}
