using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.RequestComments.Commands.UpdateComment;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class UpdateCommentDto : IMapWith<UpdateRequestCommentCommand>
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;
    public RequestCommentStatus Status { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdateCommentDto, UpdateRequestCommentCommand>();
}