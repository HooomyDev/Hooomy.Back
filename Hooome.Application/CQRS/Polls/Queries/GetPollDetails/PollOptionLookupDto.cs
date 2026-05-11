using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollDetails;

public class PollOptionLookupDto : IMapWith<PollOption>
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public int VoteCount { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<PollOption, PollOptionLookupDto>()
            .ForMember(dest => dest.VoteCount, opt => opt.MapFrom(src => src.Votes.Count));
    }
}