using FluentValidation;

namespace Hooome.Application.Chats.Commands.CreateChat;

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
        RuleFor(c => c.ResidentId).NotEqual(Guid.Empty);
        RuleFor(c => c.CompanyId).NotEqual(Guid.Empty);
    }
}
