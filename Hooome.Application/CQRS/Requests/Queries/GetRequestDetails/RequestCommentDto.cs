using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestDetails;

public class RequestCommentDto : IMapWith<RequestComment>
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }
    public string Text { get; set; } = null!;
    public string PhotoUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<RequestComment, RequestCommentDto>();
}