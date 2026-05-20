using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.RequestReviews.Commands.CreateReview;

namespace Hooome.WebApi.Models;

public class CreateReviewDto : IMapWith<CreateReviewCommand>
{
    public int Score { get; set; }
    public string? Text { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateReviewDto, CreateReviewCommand>();
}