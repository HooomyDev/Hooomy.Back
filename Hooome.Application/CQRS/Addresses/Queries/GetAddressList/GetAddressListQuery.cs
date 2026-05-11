using MediatR;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressList;

public class GetAddressListQuery : IRequest<AddressListVm>
{
    public string Query { get; set; } = string.Empty;
}
