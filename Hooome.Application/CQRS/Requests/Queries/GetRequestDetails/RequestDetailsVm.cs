using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDetails;

public class RequestDetailsVm : IMapWith<Request>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public RequestCategory Category { get; set; } = RequestCategory.Other;
    public IList<string> ImagesUrls { get; set; } = [];
    public ReviewDto? Review { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Request, RequestDetailsVm>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => $"{src.Address.Street}, {src.Address.HouseNumber}"))
            .ForMember(dest => dest.Review,
                opt => opt.MapFrom(src => src.Review));
    }
}

public class ReviewDto : IMapWith<RequestReview>
{
    public Guid Id { get; set; }
    public int Score { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<RequestReview, ReviewDto>();
}