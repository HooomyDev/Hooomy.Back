using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Streets.Queries.GetStreetList;

public class GetStreetListQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetStreetListQuery, StreetListVm>
{
    public async Task<StreetListVm> Handle(GetStreetListQuery request,
           CancellationToken cancellationToken)
    {
        IList<StreetListDto> streets = [];

        if(string.IsNullOrWhiteSpace(request.Query))
        {
            streets = await dbContext.Streets
                .Take(15)
                .ProjectTo<StreetListDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        else
        {
            streets = await dbContext.Streets
                .Where(s => s.Title.StartsWith(request.Query.Trim(), StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(s => s.Title)
                .Take(15)
                .ProjectTo<StreetListDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        return new StreetListVm
        {
            Streets = streets
        };
    }
}
