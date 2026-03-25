using FluentValidation;

namespace Hooome.Application.Messages.Queries.GetMessageList;

public class GetMessageListQueryValidator 
    : AbstractValidator<GetMessageListQuery>
{
    public GetMessageListQueryValidator()
    {
        RuleFor(x => x.ChatId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}