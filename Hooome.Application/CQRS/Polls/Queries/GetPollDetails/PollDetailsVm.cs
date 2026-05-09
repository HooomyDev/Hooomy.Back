using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollDetails;

public class PollDetailsVm : IMapWith<Poll>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public int VoteCount { get; set; }
    public PollType Type { get; set; }
    public IList<PollOptionLookupDto> Options { get; set; } = [];
    public IList<Guid>? UserVotes { get; set; }
    public bool UserHasVoted => UserVotes != null && UserVotes.Any(x => x != Guid.Empty);

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Poll, PollDetailsVm>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.VoteCount, opt => opt.MapFrom(src => src.Votes.Count));

        profile.CreateMap<PollOption, PollOptionLookupDto>();
    }
}
