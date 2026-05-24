using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressDetails;

public class CompanyDto : IMapWith<Company>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string WorkingHours { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<Company, CompanyDto>();
}