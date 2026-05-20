using FluentValidation;

namespace Hooome.Application.CQRS.RequestReviews.Commands.CreateReview;

public class CreateReviewCommandValidator
    : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(rw => rw.RequestId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(rw => rw.UserId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(rw => rw.Score)
            .NotEmpty()
            .InclusiveBetween(1, 5);
        RuleFor(rw => rw.Text)
            .MaximumLength(2000)
            .When(rw => !string.IsNullOrWhiteSpace(rw.Text));
    }
}