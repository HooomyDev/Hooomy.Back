using FluentValidation;

namespace Hooome.Application.CQRS.Addresses.Commands.CreateAddress;

public class CreateAddressCommandValidator 
    : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(a => a.Address).NotEmpty();
        RuleFor(a => a.Latitude).NotEmpty();
        RuleFor(a => a.Longitude).NotEmpty();
    }
}
