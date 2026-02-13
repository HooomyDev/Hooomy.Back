using FluentValidation;

namespace Hooome.Application.Complaints.Queries.GetComplaintList;

public class GetComplaintListQueryValidator
    : AbstractValidator<GetComplaintListQuery>
{
    public GetComplaintListQueryValidator()
    {
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
    }
}
