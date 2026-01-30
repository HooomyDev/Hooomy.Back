using MediatR;

namespace Hooome.Application.Streets.Queries.GetStreetList;

public class GetStreetListQuery : IRequest<StreetListVm>
{
    public string Query { get; set; } = string.Empty;
}
