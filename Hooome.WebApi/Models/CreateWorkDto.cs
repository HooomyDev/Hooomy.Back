using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Works.Commands.CreateWork;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class CreateWorkDto : IMapWith<CreateWorkCommand>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid AddressId { get; set; } 
    public RequestCategory Category { get; set; }
    public WorkSeriousness Seriousness { get; set; }
    public DateTime PlannedStartTime { get; set; }
    public DateTime PlannedEndTime { get; set; }

    public void Mapping(Profile profile) 
        => profile.CreateMap<CreateWorkDto, CreateWorkCommand>();
}
