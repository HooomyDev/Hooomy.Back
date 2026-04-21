using FluentValidation;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintList;

public class GetComplaintListQueryValidator
    : AbstractValidator<GetComplaintListQuery>
{
    public GetComplaintListQueryValidator()
    {
    }
}
