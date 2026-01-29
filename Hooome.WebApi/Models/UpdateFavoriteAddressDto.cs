using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.FavoriteAddresses.Commands.UpdateFavoriteAddress;

namespace Hooome.WebApi.Models;

public class UpdateFavoriteAddressDto 
    : IMapWith<UpdateFavoriteAddressCommand>
{
    public Guid Id { get; set; }
    public string Street { get; set; } = null!;
    public int House { get; set; }
    public string Pseudonym { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdateFavoriteAddressDto, UpdateFavoriteAddressCommand>();
}
