using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Works.Commands.UpdateWork;

public class UpdateWorkCommand : IRequest, IMapWith<Work>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public RequestCategory Category { get; set; }
    public WorkSeriousness Seriousness { get; set; }
    public DateTime PlannedStartTime { get; set; }
    public DateTime PlannedEndTime { get; set; }
    public DateTime? FactStartTime { get; set; }
    public DateTime? FactEndTime { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdateWorkCommand, Work>();
}
