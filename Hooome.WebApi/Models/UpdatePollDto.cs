using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Polls.Commands.UpdatePoll;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class UpdatePollDto : IMapWith<UpdatePollCommand>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public PollStatus Status { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdatePollDto, UpdatePollCommand>();
}