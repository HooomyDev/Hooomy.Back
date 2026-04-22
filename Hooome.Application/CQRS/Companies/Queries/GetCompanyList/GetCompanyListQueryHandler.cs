using AutoMapper;
using Hooome.Application.Interfaces;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyList;

public class GetCompanyListQueryHandler(ICompanyRepository companyRepo,
    IMapper mapper, 
    IMinioService minioService)
    : IRequestHandler<GetCompanyListQuery, CompanyListVm>
{
    public async Task<CompanyListVm> Handle(GetCompanyListQuery request, CancellationToken cancellationToken)
    {
        var companies = await companyRepo.GetAll(cancellationToken);

        var companyDtos = mapper.Map<List<CompanyListLookupDto>>(companies);

        foreach (var company in companyDtos)
        {
            company.LogoUrl = minioService.GetUrl(ImageType.Company, company.LogoUrl);
        }

        return new CompanyListVm { Companies = companyDtos };
    }
}
