using AutoMapper;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Queries.GetPollList;

public class GetPollListQueryHandler(IPollRepository pollRepo, IMapper mapper)
    : IRequestHandler<GetPollListQuery, PollListVm>
{
    public async Task<PollListVm> Handle(GetPollListQuery request, CancellationToken cancellationToken)
    {
        var (polls, totalCount) = await pollRepo.GetFilteredPolls(
            title: request.Title, 
            type: request.Type, 
            status: request.Status, 
            page: request.Page,
            pageSize: request.PageSize,
            companyId: request.CompanyId,
            cancellationToken: cancellationToken);

        var pollDtos = mapper.Map<List<PollListLookupDto>>(polls);

        return new PollListVm 
        { 
            Polls = pollDtos,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            Status = request.Status,
            Type = request.Type,
        };
    }
}
