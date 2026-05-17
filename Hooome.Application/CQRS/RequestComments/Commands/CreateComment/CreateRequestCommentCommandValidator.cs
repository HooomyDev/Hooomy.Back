using FluentValidation;

namespace Hooome.Application.CQRS.RequestComments.Commands.CreateComment;

public class CreateRequestCommentCommandValidator
    : AbstractValidator<CreateRequestCommentCommand>
{
    public CreateRequestCommentCommandValidator()
    {
        RuleFor(r => r.UserId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(r => r.RequestId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(r => r.Text).NotEmpty().MaximumLength(1000);
    }
}