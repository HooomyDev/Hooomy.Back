using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestListWithPagination;

public class GetRequestListWithPaginationQueryValidator
    : AbstractValidator<GetRequestListWithPaginationQuery>
{
    public GetRequestListWithPaginationQueryValidator()
    {
        RuleFor(r => r.Title)
            .MaximumLength(250);
    }
}