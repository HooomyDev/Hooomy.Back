using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Commands.UpdateComment;

public class UpdateRequestCommentCommand : IRequest, IMapWith<RequestComment>
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;
    public RequestCommentStatus Status { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdateRequestCommentCommand, RequestComment>();
}
