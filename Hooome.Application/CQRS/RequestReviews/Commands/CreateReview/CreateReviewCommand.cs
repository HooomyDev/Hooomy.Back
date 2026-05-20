using AutoMapper;
using MediatR;

namespace Hooome.Application.CQRS.RequestReviews.Commands.CreateReview;

public class CreateReviewCommand : IRequest
{
    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
    public string? Text { get; set; }
}
