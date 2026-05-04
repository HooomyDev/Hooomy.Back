using FluentValidation;

namespace Hooome.Application.CQRS.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandValidator
    : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyCommandValidator()
    {
        RuleFor(c => c.CompanyId).NotEmpty().NotEqual(Guid.Empty);
    }
}