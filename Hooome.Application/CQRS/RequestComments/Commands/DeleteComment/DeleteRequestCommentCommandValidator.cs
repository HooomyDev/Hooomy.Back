using FluentValidation;

namespace Hooome.Application.CQRS.RequestComments.Commands.DeleteComment;

public class DeleteRequestCommentCommandValidator
    : AbstractValidator<DeleteRequestCommentCommand>
{
    public DeleteRequestCommentCommandValidator()
    {
        RuleFor(cc => cc.CommentId).NotEmpty().NotEqual(Guid.Empty);
    }
}