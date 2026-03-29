using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Streets.Queries.GetStreetList;

public class StreetListDto : IMapWith<Street>
{
    public string Title { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<Street, StreetListDto>();
}
