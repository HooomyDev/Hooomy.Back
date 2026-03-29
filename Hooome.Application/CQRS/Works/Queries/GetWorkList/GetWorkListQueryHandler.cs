using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Works.Queries.GetWorkList;

public class GetWorkListQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetWorkListQuery, GetWorkListVm>
{
    public async Task<GetWorkListVm> Handle(GetWorkListQuery request, CancellationToken cancellationToken)
    {
        var works = await (
            from w in dbContext.Works
            join a in dbContext.FavoriteAddresses
                on new { w.Street, w.House } equals new { a.Street, a.House }
            where a.UserID == request.UserId
            select w
        )
        .ProjectTo<WorkListLookupDto>(mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

        return new GetWorkListVm { Works = works };
    }
}
