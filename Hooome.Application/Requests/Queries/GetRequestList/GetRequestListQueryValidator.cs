using FluentValidation;

namespace Hooome.Application.Requests.Queries.GetRequestList;

public class GetRequestListQueryValidator 
    : AbstractValidator<GetRequestListQuery>
{
    public GetRequestListQueryValidator()
    {
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
    }
}
