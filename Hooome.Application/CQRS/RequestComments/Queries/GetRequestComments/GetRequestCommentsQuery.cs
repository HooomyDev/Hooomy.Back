using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Queries.GetRequestComments;

public class GetRequestCommentsQuery : IRequest<RequestCommentsVm>
{
    public Guid? RequestId { get; set; }
    public string? Text { get; set; }
    public RequestCommentStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}
