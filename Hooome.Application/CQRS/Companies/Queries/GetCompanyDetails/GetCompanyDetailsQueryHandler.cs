using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyDetails;

public class GetCompanyDetailsQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetCompanyDetailsQuery, CompanyDetailsVm>
{
    public async Task<CompanyDetailsVm> Handle(GetCompanyDetailsQuery request, CancellationToken cancellationToken)
    {
        var company = await dbContext.Companies
            .FindAsync([request.CompanyId], cancellationToken) 
            ?? throw new NotFoundException(nameof(Company), request.CompanyId);

        return mapper.Map<CompanyDetailsVm>(company);
    }
}
