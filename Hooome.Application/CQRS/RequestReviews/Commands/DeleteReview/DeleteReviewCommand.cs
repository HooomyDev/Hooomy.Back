using MediatR;

namespace Hooome.Application.CQRS.RequestReviews.Commands.DeleteReview;

public class DeleteReviewCommand : IRequest
{
    public Guid ReviewId { get; set; }
}
