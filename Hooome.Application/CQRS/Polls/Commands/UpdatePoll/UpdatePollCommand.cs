using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.UpdatePoll;

public class UpdatePollCommand : IRequest, IMapWith<Poll>
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public PollStatus Status { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdatePollCommand, Poll>();
    }
}
