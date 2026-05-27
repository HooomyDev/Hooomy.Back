using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;

public class GetRequestDailyStatisticQuery : IRequest<RequestDailyStatisticVm>
{
    public Period Period { get; set; } = Period.Week;
    public Guid? CompanyId { get; set; }
}
