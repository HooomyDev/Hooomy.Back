using FluentValidation;

namespace Hooome.Application.CQRS.RequestComments.Commands.UpdateComment;

public class UpdateRequestCommentCommandValidator
    : AbstractValidator<UpdateRequestCommentCommand>
{
    public UpdateRequestCommentCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(c => c.Text).MaximumLength(1000);
    }
}