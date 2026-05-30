using FluentValidation;

namespace Hooome.Application.CQRS.Works.Queries.GetWorkListForEmployee;

public class GetWorkListForEmployeeQueryValidator
    : AbstractValidator<GetWorkListForEmployeeQuery>
{
    public GetWorkListForEmployeeQueryValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty().NotEqual(Guid.Empty);
    }
}
