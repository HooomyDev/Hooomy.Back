using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestList;

public class RequestListDto : IMapWith<Request>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public DateTime? CreatedAt { get; set; }

    public void Mapping(Profile profile) 
        => profile.CreateMap<Request, RequestListDto>();
}
