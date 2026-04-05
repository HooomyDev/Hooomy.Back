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
        var works = await dbContext.Works
            .Include(w => w.Address)
                .ThenInclude(a => a.FavoriteAddresses)
            .Where(w => w.Address.FavoriteAddresses.Any(fa => fa.UserID == request.UserId))
            .ProjectTo<WorkListLookupDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetWorkListVm { Works = works };
    }
}
