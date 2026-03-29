using FluentValidation;

namespace Hooome.Application.CQRS.Works.Queries.GetWorkList;

public class GetWorkListQueryValidator : AbstractValidator<GetWorkListQuery>
{
    public GetWorkListQueryValidator()
    {
        RuleFor(q => q.UserId).NotEmpty();
    }
}
