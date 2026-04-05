namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressList;

public class AddressListVm
{
    public IList<AddressListLookupDto> Addresses { get; set; } = [];
}
