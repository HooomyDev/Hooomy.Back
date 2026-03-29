using FluentValidation;

namespace Hooome.Application.CQRS.Streets.Queries.GetStreetList;

public class GetStreetListQueryValidator 
    : AbstractValidator<GetStreetListQuery>
{
    public GetStreetListQueryValidator()
    {
        RuleFor(q => q.Query).MaximumLength(50);
    }
}
