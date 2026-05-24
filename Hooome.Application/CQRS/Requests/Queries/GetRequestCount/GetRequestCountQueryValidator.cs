using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCount;

public class GetRequestCountQueryValidator 
    : AbstractValidator<GetRequestCountQuery>
{
    public GetRequestCountQueryValidator()
    {
        RuleFor(r => r.Status).IsInEnum();
    }
}