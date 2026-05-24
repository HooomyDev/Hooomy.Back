using FluentValidation;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressDetails;

public class GetAddressDetailsQueryValidator
    : AbstractValidator<GetAddressDetailsQuery>
{
    public GetAddressDetailsQueryValidator()
    {
        RuleFor(a => a.AddressId).NotEmpty().NotEqual(Guid.Empty);
    }
}