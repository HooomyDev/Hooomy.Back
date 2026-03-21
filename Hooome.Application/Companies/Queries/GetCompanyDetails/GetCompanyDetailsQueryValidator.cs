using FluentValidation;

namespace Hooome.Application.Companies.Queries.GetCompanyDetails;

public class GetCompanyDetailsQueryValidator 
    : AbstractValidator<GetCompanyDetailsQuery>
{
    public GetCompanyDetailsQueryValidator()
    {
        RuleFor(x => x.CompanyId).NotEqual(Guid.Empty);
    }
}