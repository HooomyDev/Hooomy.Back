using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Commands.CreateComment;

public class CreateRequestCommentCommandValidor
    : AbstractValidator<CreateRequestCommentCommand>
{
    public CreateRequestCommentCommandValidor()
    {
        RuleFor(r => r.UserId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(r => r.RequestId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(r => r.SenderName).NotEmpty();
        RuleFor(r => r.Text).NotEmpty().MaximumLength(1000);
    }
}