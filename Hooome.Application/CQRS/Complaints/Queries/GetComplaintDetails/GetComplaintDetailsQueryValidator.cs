using FluentValidation;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintDetails;

public class GetComplaintDetailsQueryValidator
    : AbstractValidator<GetComplaintDetailsQuery>
{
    public GetComplaintDetailsQueryValidator()
    {
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
        RuleFor(c => c.Id).NotEqual(Guid.Empty);
    }
}