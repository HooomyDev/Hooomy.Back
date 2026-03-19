using FluentValidation;

namespace Hooome.Application.Chats.Queries.GetChatList;

public class GetChatListQueryValidator : AbstractValidator<GetChatListQuery>
{
    public GetChatListQueryValidator()
    {
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
    }
}