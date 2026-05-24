using FluentValidation;

namespace Hooome.Application.CQRS.Works.Queries.GetWorkListForEmployee;

public class GetWorkListForEmployeeQueryValidator
    : AbstractValidator<GetWorkListForEmployeeQuery>
{
    public GetWorkListForEmployeeQueryValidator()
    {
        
    }
}
