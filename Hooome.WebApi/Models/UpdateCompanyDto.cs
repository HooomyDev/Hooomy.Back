using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Companies.Commands.UpdateCompany;

namespace Hooome.WebApi.Models;

public class UpdateCompanyDto : IMapWith<UpdateCompanyCommand>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string WorkingHours { get; set; } = null!;
    public Guid AddressId { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdateCompanyDto, UpdateCompanyCommand>();
}