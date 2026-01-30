using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.Works.Queries.GetWorkList;

public class WorkListLookupDto : IMapWith<Work>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Street { get; set; } = null!;
    public int House { get; set; }
    public RequestCategory Category { get; set; }
    public WorkSeriousness Seriousness { get; set; }
    public DateTime PlannedStartTime { get; set; }
    public DateTime PlannedEndTime { get; set; }
    public DateTime? FactStartTime { get; set; }
    public DateTime? FactEndTime { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Work, WorkListLookupDto>();
}