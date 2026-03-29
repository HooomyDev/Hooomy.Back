using FluentValidation;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Messages.Commands.CreateMessage;

public class CreateMessageCommandValidator
    : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageCommandValidator()
    {
        RuleFor(m => m.ChatId).NotEqual(Guid.Empty);
        RuleFor(m => m.SenderId).NotEqual(Guid.Empty);
        RuleFor(m => m.SenderName)
            .NotEmpty()
            .MaximumLength(300);
        RuleFor(m => m.SenderType)
            .NotEmpty()
            .IsInEnum()
            .NotEqual(SenderType.Unknown);
        RuleFor(m => m.MessageType)
            .NotEmpty()
            .IsInEnum()
            .NotEqual(MessageType.Unknown);
        RuleFor(m => m.Content)
            .NotEmpty()
            .MaximumLength(5000);
    }
}
