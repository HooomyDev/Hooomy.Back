using MediatR;

namespace Hooome.Application.CQRS.Works.Queries.GetWorkList;

public class GetWorkListQuery : IRequest<GetWorkListVm>
{
    public Guid UserId { get; set; }
}
