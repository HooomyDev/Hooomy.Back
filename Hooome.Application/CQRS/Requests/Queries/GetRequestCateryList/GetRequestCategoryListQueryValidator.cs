using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCateryList;

public class GetRequestCategoryListQueryValidator 
    : AbstractValidator<GetRequestCategoryListQuery>
{
    public GetRequestCategoryListQueryValidator()
    {
        
    }
}