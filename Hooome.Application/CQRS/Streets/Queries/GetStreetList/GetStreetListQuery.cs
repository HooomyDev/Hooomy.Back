using MediatR;

namespace Hooome.Application.CQRS.Streets.Queries.GetStreetList;

public class GetStreetListQuery : IRequest<StreetListVm>
{
    public string Query { get; set; } = string.Empty;
}
