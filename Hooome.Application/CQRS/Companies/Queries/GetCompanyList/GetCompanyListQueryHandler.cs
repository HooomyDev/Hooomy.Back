using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyList;

public class GetCompanyListQueryHandler(IHooomeDbContext context, IMapper mapper)
    : IRequestHandler<GetCompanyListQuery, CompanyListVm>
{
    public async Task<CompanyListVm> Handle(GetCompanyListQuery request, CancellationToken cancellationToken)
    {
        var companies = await context.Companies
            .Take(50)
            .ProjectTo<CompanyListLookupDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new CompanyListVm { Companies = companies };
    }
}
