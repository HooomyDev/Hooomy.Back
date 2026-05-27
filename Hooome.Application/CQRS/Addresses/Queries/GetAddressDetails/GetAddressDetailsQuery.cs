using MediatR;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressDetails;

public class GetAddressDetailsQuery : IRequest<AddressDetailsVm>
{
    public Guid AddressId { get; set; }
}
