using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Commands.DeleteComment;

public class DeleteRequestCommentCommandHandler(IRequestCommentRepository requestCommentRepo)
    : IRequestHandler<DeleteRequestCommentCommand>
{
    public async Task Handle(DeleteRequestCommentCommand request, CancellationToken cancellationToken)
    {
        var requestComment = await requestCommentRepo.GetById(request.CommentId, cancellationToken)
            ?? throw new NotFoundException(nameof(RequestComment), request.CommentId);

        await requestCommentRepo.Delete(requestComment, cancellationToken);
    }
}