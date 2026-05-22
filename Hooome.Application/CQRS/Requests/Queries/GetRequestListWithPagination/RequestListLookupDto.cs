using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestListWithPagination;

public class RequestListLookupDto : IMapWith<Request>
{
    public Guid Id { get; set; }
    public Guid UserID { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public RequestCategory Category { get; set; } = RequestCategory.Other;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Request, RequestListLookupDto >()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                $"{src.Address.Street}, {src.Address.HouseNumber}"));
    }
}