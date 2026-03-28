using FluentValidation;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollList;

public class GetPollListQueryValidator 
    : AbstractValidator<GetPollListQuery>
{
    public GetPollListQueryValidator()
    {
        RuleFor(x => x.Page)
           .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);
    }
}