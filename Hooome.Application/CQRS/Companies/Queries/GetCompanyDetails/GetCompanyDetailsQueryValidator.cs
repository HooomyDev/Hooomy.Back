using FluentValidation;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyDetails;

public class GetCompanyDetailsQueryValidator 
    : AbstractValidator<GetCompanyDetailsQuery>
{
    public GetCompanyDetailsQueryValidator()
    {
        RuleFor(x => x.CompanyId).NotEqual(Guid.Empty);
    }
}