using FluentValidation;

namespace Hooome.Application.CQRS.RequestComments.Commands.UploadCommentImages;

public class UploadRequestCommentImageCommandValidator
    : AbstractValidator<UploadRequestCommentImageCommand>
{
    public UploadRequestCommentImageCommandValidator()
    {
        RuleFor(x => x.RequestCommentId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(x => x.Files).NotEmpty();
    }
}