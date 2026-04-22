using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyList;

public class CompanyListLookupDto : IMapWith<Company>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string LogoUrl { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Company, CompanyListLookupDto>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address != null
                    ? $"{src.Address.Street}, {src.Address.HouseNumber}"
                    : string.Empty));
    }
}
