using FluentValidation;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.RequestReviews.Commands.CreateReview;

public class CreateReviewCommandHandler(IRequestRepository requestRepo,
    IRequestReviewRepository requestReviewRepo)
    : IRequestHandler<CreateReviewCommand>
{
    public async Task Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var existRequest = await requestRepo.GetById(request.RequestId, cancellationToken) 
            ?? throw new NotFoundException(nameof(Request), request.RequestId);
        
        if(existRequest.Review is not null) 
            throw new ValidationException("Review already exists");

        if (existRequest.Status != Domain.Enums.RequestStatus.Completed)
            throw new ValidationException("You can only review completed requests");
        
        var requestReview = new RequestReview()
        {
            Id = Guid.NewGuid(),
            RequestId = request.RequestId,
            UserId = request.UserId,
            Score = request.Score,
            Text = request.Text,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            DeletedAt = null,
        };

        await requestReviewRepo.Create(requestReview, cancellationToken);
    }
}
