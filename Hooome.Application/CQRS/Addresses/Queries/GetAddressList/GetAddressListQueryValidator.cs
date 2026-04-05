using FluentValidation;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressList;

public class GetAddressListQueryValidator 
    : AbstractValidator<GetAddressListQuery>
{
    public GetAddressListQueryValidator()
    {
        RuleFor(q => q.Query).MaximumLength(150);
    }
}