using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.Requests.Queries.GetRequestDetails;

public class RequestDetailsVm : IMapWith<Request>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public RequestCategory Category { get; set; } = RequestCategory.Other;
    public string PhotoUrl { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Mapping(Profile profile) 
        => profile.CreateMap<Request, RequestDetailsVm>();
}