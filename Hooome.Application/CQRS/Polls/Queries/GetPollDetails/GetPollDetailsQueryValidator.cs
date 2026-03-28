using FluentValidation;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollDetails;

public class GetPollDetailsQueryValidator
    : AbstractValidator<GetPollDetailsQuery>
{
    public GetPollDetailsQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotEqual(Guid.Empty);
    }
}