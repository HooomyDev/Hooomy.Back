using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Works.Queries.GetWorkList;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressDetails;

public class AddressDetailsVm : IMapWith<Address>
{
    public Guid Id { get; set; }
    public string Street { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;
    public CompanyDto? Company { get; set; } 
    public IList<WorkListLookupDto> Works { get; set; } = [];

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Address, AddressDetailsVm>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.ServicedByCompany))
            .ForMember(dest => dest.Works, opt => opt.MapFrom(src => src.Works));
    }
}
