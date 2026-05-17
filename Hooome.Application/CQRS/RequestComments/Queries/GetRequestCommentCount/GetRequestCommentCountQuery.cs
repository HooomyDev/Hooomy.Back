using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Queries.GetRequestCommentCount;

public class GetRequestCommentCountQuery : IRequest<int>
{
    public Guid? RequestId { get; set; }
    public string? Filter { get; set; }
}
