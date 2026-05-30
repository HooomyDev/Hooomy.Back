using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Works.Queries.GetWorkListForEmployee;

public class GetWorkListForEmployeeQuery : IRequest<GetWorkListWithPaginationVm>
{
    public Guid CompanyId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public RequestCategory? Category { get; set; }
    public WorkSeriousness? Seriousness { get; set; }
    public Guid? AddressId { get; set; }
    public string? SearchTitle { get; set; }
}
