using FluentValidation;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatDetailsQuery;

public class GetChatDetailsQueryValidator
    : AbstractValidator<GetChatDetailsQuery>
{
    public GetChatDetailsQueryValidator()
    {
        RuleFor(x => x.ResidentId).NotEqual(Guid.Empty);
        RuleFor(x => x.ChatId).NotEqual(Guid.Empty);
    }
}