using FluentValidation;

namespace Hooome.Application.CQRS.RequestComments.Queries.GetRequestCommentCount;

public class GetRequestCommentCountQueryValidator
    : AbstractValidator<GetRequestCommentCountQuery>
{
    public GetRequestCommentCountQueryValidator()
    {
        
    }
}