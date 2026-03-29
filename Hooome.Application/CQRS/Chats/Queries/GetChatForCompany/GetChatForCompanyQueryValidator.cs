using FluentValidation;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatForCompany;

public class GetChatForCompanyQueryValidator 
    : AbstractValidator<GetChatForCompanyQuery>
{
    public GetChatForCompanyQueryValidator()
    {
        RuleFor(x => x.CompanyId).NotEqual(Guid.Empty);
    }
}
