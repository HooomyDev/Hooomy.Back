using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCategoryList;

public class GetRequestCategoryListQueryValidator 
    : AbstractValidator<GetRequestCategoryListQuery>
{
    public GetRequestCategoryListQueryValidator()
    {
        
    }
}