using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestList;

public class GetRequestListQuery : IRequest<RequestListVm>
{
    public Guid UserId { get; set; }

    public RequestCategory? RequestCategory { get; set; } 
    public RequestStatus? RequestStatus { get; set; } 
    public string? SearchTitle { get; set; }
    public Guid? AddressId { get; set; }
}
