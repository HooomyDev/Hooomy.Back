using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.RequestReviews.Commands.DeleteReview;

public class DeleteReviewCommandHandler(IRequestReviewRepository requestReviewRepo)
    : IRequestHandler<DeleteReviewCommand>
{
    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await requestReviewRepo.GetById(request.ReviewId)
            ?? throw new NotFoundException(nameof(RequestReview), request.ReviewId);

        await requestReviewRepo.Delete(review, cancellationToken);
    }
}
