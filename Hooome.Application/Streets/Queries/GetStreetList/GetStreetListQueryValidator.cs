using FluentValidation;

namespace Hooome.Application.Streets.Queries.GetStreetList;

public class GetStreetListQueryValidator 
    : AbstractValidator<GetStreetListQuery>
{
    public GetStreetListQueryValidator()
    {
        RuleFor(q => q.Query).MaximumLength(50);
    }
}
