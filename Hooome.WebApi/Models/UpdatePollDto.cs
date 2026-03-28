using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Polls.Commands.UpdatePoll;

namespace Hooome.WebApi.Models;

public class UpdatePollDto : IMapWith<UpdatePollCommand>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsActive { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdatePollDto, UpdatePollCommand>();
}