using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyDetails;

public class CompanyDetailsVm : IMapWith<Company>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string WorkingHours { get; set; } = null!;
    public string Address { get; set; } = null!;
    public List<AddressDto> Addresses { get; set; } = [];
    public string LogoUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Company, CompanyDetailsVm>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => $"{src.Address.Street}, {src.Address.HouseNumber}"))
            .ForMember(dest => dest.Addresses,
                opt => opt.MapFrom(src => src.ServedAddresses));
    }
}
