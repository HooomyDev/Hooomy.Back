using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDetails;

public class GetRequestDetailsQueryValidator 
    : AbstractValidator<GetRequestDetailsQuery>
{
    public GetRequestDetailsQueryValidator()
    {
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
        RuleFor(c => c.Id).NotEqual(Guid.Empty);
    }
}
