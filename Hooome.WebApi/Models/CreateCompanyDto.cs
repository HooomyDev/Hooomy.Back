using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Companies.Commands.CreateCompany;

namespace Hooome.WebApi.Models;

public class CreateCompanyDto : IMapWith<CreateCompanyCommand>
{
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? WorkingHours { get; set; }
    public Guid? AddressId { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateCompanyDto, CreateCompanyCommand>();
}