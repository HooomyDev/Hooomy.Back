using FluentValidation;

namespace Hooome.Application.CQRS.Companies.Commands.RemoveAddress;

public class RemoveAddressCommandValidator
    : AbstractValidator<RemoveAddressCommand>
{
    public RemoveAddressCommandValidator()
    {
        RuleFor(c => c.CompanyId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(c => c.AddressId).NotEmpty().NotEqual(Guid.Empty);
    }
}