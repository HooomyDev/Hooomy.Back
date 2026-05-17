using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Commands.DeleteComment;

public class DeleteRequestCommentCommand : IRequest
{
    public Guid CommentId { get; set; }
}
