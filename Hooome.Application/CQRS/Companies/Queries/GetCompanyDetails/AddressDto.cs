using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyDetails;

public class AddressDto : IMapWith<Address>
{ 
    public Guid Id { get; set; }
    public string FullAddress { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Address, AddressDto>()
            .ForMember(dest => dest.FullAddress,
                opt => opt.MapFrom(src => $"{src.Street}, {src.HouseNumber}"));
    }
}
