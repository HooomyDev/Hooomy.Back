using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.FavoriteAddresses.Queries.GetFavoriteAddressList;

public class FavoriteAddressListDto : IMapWith<FavoriteAddress>
{
    public Guid Id { get; set; }
    public string Street { get; set; } = null!;
    public string House { get; set; } = null!;
    public string Pseudonym { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<FavoriteAddress, FavoriteAddressListDto>();
}