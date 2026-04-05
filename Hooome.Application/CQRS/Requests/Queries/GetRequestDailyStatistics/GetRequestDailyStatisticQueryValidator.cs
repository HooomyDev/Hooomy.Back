using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDailyStatistics;

public class GetRequestDailyStatisticQueryValidator
    : AbstractValidator<GetRequestDailyStatisticQuery>
{
    public GetRequestDailyStatisticQueryValidator()
    {
        RuleFor(x => x.Period)
            .NotEmpty();
    }
}
