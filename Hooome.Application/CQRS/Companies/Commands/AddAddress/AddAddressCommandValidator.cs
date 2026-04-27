using FluentValidation;

namespace Hooome.Application.CQRS.Companies.Commands.AddAddress;

public class AddAddressCommandValidator 
    : AbstractValidator<AddAddressCommand>
{
    public AddAddressCommandValidator()
    {
        RuleFor(c => c.CompanyId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(c => c.AddressId).NotEmpty().NotEqual(Guid.Empty);
    }
}