using FluentValidation;

namespace Hooome.Application.CQRS.Companies.Commands.CreateCompany;

public class CreateCompanyCommandValidator
    : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MinimumLength(2);

        RuleFor(c => c.Phone)
            .MaximumLength(20)
            .Matches(@"^[\+0-9\s\-\(\)]+$")
            .When(c => !string.IsNullOrEmpty(c.Phone));

        RuleFor(c => c.Email)
            .EmailAddress()
            .MaximumLength(100)
            .When(c => !string.IsNullOrEmpty(c.Email));

        RuleFor(c => c.WorkingHours)
            .MaximumLength(200)
            .Matches(@"^[0-9\-:\s,]+$").WithMessage("Incorrect format (example: 09:00-18:00)")
            .When(c => !string.IsNullOrEmpty(c.WorkingHours));

        RuleFor(c => c.AddressId)
            .Must(id => id != Guid.Empty)
            .When(c => c.AddressId.HasValue);
    }
}