using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.FavoriteAddresses.Commands.CreateFavoriteAddress;

namespace Hooome.WebApi.Models;

public class CreateFavoriteAddressDto 
    : IMapWith<CreateFavoriteAddressCommand>
{
    public Guid AddressId { get; set; }
    public string Pseudonym { get; set; } = null!;

    public void Mapping(Profile profile) 
        => profile.CreateMap<CreateFavoriteAddressDto, CreateFavoriteAddressCommand>();
}
