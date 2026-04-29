using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Queries.GetRequestListWithPagination;

public class GetRequestListWithPaginationQuery : IRequest<RequestListWithPaginationVm>
{
    public string? Title { get; set; }
    public Guid? CompanyId { get; set; }
    public RequestStatus Status { get; set; }
    public RequestCategory Category { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
