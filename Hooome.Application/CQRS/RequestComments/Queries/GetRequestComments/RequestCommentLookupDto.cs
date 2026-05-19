using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.RequestComments.Queries.GetRequestComments;

public class RequestCommentLookupDto : IMapWith<RequestComment>
{
    public Guid Id { get; set; }
    public string SenderName { get; set; } = null!;
    public Guid CompanyId { get; set; }
    public string Text { get; set; } = null!;
    public RequestCommentStatus Status { get; set; }
    public List<string> PhotoUrls { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RequestComment, RequestCommentLookupDto>()
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.PhotoUrls, opt => opt.Ignore());
    }
}