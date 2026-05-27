using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestList;

public class RequestListDto : IMapWith<Request>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public RequestCategory Category { get; set; } = RequestCategory.None;
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public string Address { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Request, RequestListDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                $"{src.Address.Street}, {src.Address.HouseNumber}"));
    }
}
