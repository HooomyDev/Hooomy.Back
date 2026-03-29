using FluentValidation;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyList;

public class GetCompanyListQueryValidator
    : AbstractValidator<GetCompanyListQuery>
{
    public GetCompanyListQueryValidator()
    {
        
    }
}