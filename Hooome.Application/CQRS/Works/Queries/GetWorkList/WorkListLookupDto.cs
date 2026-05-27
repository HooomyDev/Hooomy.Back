using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Works.Queries.GetWorkList;

public class WorkListLookupDto : IMapWith<Work>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public RequestCategory Category { get; set; }
    public WorkSeriousness Seriousness { get; set; }
    public DateTime PlannedStartTime { get; set; }
    public DateTime PlannedEndTime { get; set; }
    public DateTime? FactStartTime { get; set; }
    public DateTime? FactEndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Work, WorkListLookupDto>()
            .ForMember(dest => dest.Address, 
            opt => opt.MapFrom(src => $"{src.Address.Street}, {src.Address.HouseNumber}"));
    }
}