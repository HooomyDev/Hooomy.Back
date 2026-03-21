using FluentValidation;

namespace Hooome.Application.Chats.Queries.GetChatDetailsQuery;

public class GetChatDetailsQueryValidator
    : AbstractValidator<GetChatDetailsQuery>
{
    public GetChatDetailsQueryValidator()
    {
        RuleFor(x => x.ResidentId).NotEqual(Guid.Empty);
        RuleFor(x => x.CompanyId).NotEqual(Guid.Empty);
    }
}