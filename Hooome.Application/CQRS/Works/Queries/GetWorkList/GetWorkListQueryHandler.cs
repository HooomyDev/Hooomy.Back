using AutoMapper;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Works.Queries.GetWorkList;

public class GetWorkListQueryHandler(IWorkRepository workRepo, IMapper mapper)
    : IRequestHandler<GetWorkListQuery, GetWorkListVm>
{
    public async Task<GetWorkListVm> Handle(GetWorkListQuery request, CancellationToken cancellationToken)
    {
        var works = await workRepo.GetByUserIdAsync(request.UserId, cancellationToken);

        var workDtos = mapper.Map<List<WorkListLookupDto>>(works);

        return new GetWorkListVm { Works = workDtos };
    }
}
