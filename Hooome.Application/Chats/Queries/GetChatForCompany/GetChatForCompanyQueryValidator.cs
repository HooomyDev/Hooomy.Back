using FluentValidation;

namespace Hooome.Application.Chats.Queries.GetChatForCompany;

public class GetChatForCompanyQueryValidator 
    : AbstractValidator<GetChatForCompanyQuery>
{
    public GetChatForCompanyQueryValidator()
    {
        RuleFor(x => x.CompanyId).NotEqual(Guid.Empty);
    }
}
