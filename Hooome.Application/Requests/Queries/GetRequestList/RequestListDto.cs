using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.Requests.Queries.GetRequestList;

public class RequestListDto : IMapWith<Request>
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public RequestCategory Category { get; set; } = RequestCategory.Other;

    public void Mapping(Profile profile) 
        => profile.CreateMap<Request, RequestListDto>();
}
