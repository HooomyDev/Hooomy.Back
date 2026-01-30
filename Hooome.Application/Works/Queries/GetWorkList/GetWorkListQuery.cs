using MediatR;

namespace Hooome.Application.Works.Queries.GetWorkList;

public class GetWorkListQuery : IRequest<GetWorkListVm>
{
    public Guid UserId { get; set; }
}
