using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Works.Commands.UpdateWork;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class UpdateWorkDto : IMapWith<UpdateWorkCommand>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid AddressId { get; set; }
    public RequestCategory Category { get; set; }
    public WorkSeriousness Seriousness { get; set; }
    public DateTime PlannedStartTime { get; set; }
    public DateTime PlannedEndTime { get; set; }
    public DateTime? FactStartTime { get; set; }
    public DateTime? FactEndTime { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdateWorkDto, UpdateWorkCommand>();
}