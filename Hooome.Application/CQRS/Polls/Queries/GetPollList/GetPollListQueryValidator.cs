using FluentValidation;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollList;

public class GetPollListQueryValidator 
    : AbstractValidator<GetPollListQuery>
{
    public GetPollListQueryValidator()
    {
        RuleFor(p => p.Page)
           .GreaterThanOrEqualTo(1);

        RuleFor(p => p.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);

        RuleFor(p => p.Title)
            .MaximumLength(500);

        RuleFor(p => p.Type)
            .IsInEnum();

        RuleFor(p => p.Status)
            .IsInEnum();
    }
}