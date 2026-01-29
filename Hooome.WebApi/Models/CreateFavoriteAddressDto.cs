using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.FavoriteAddresses.Commands.CreateFavoriteAddress;

namespace Hooome.WebApi.Models;

public class CreateFavoriteAddressDto 
    : IMapWith<CreateFavoriteAddressCommand>
{
    public string Street { get; set; } = null!;
    public int House { get; set; }
    public string Pseudonym { get; set; } = null!;

    public void Mapping(Profile profile) 
        => profile.CreateMap<CreateFavoriteAddressDto, CreateFavoriteAddressCommand>();
}
