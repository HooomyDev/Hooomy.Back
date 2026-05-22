using FluentValidation;

namespace Hooome.Application.CQRS.RequestReviews.Commands.DeleteReview;

public class DeleteReviewCommandValidator
    : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId).NotEmpty().NotEqual(Guid.Empty);
    }
}
