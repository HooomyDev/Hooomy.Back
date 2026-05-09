using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollList;

public class PollListLookupDto : IMapWith<Poll>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public int VoteCount { get; set; }
    public PollType Type { get; set; }
    public PollStatus Status { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Poll, PollListLookupDto>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.VoteCount, opt => opt.MapFrom(src => src.Votes.Count));
    }
}
