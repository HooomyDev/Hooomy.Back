using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Polls.Commands.CreatePoll;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class CreatePollDto : IMapWith<CreatePollCommand>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid CompanyId { get; set; }
    public PollType Type { get; set; }
    public List<PollOptionDto> Options { get; set; } = [];

    public void Mapping(Profile profile)
        => profile.CreateMap<CreatePollDto, CreatePollCommand>();
}