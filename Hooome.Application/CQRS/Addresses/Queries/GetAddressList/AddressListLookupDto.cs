using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressList;

public class AddressListLookupDto : IMapWith<Address>
{
    public Guid Id { get; set; }
    public string Street { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<Address, AddressListLookupDto>();
}
