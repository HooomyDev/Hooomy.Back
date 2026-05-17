using FluentValidation;

namespace Hooome.Application.CQRS.RequestComments.Queries.GetRequestComments;

public class GetRequestCommentsQueryValidator
    : AbstractValidator<GetRequestCommentsQuery>
{
    public GetRequestCommentsQueryValidator()
    {
        RuleFor(c => c.RequestId).NotEqual(Guid.Empty);
    }
}