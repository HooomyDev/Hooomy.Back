using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommand : IRequest, IMapWith<Company>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string WorkingHours { get; set; } = null!;
    public Guid AddressId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCompanyCommand, Company>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
