using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestCount;

public class GetRequestCountQueryValidator 
    : AbstractValidator<GetRequestCountQuery>
{
    public GetRequestCountQueryValidator()
    {
        
    }
}