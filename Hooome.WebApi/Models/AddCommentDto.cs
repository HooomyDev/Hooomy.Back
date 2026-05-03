using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Requests.Commands.CreateComment;

namespace Hooome.WebApi.Models;

public class AddCommentDto : IMapWith<CreateRequestCommentCommand>
{
    public Guid RequestId { get; set; }
    public string Text { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<AddCommentDto, CreateRequestCommentCommand>();
}