using AutoMapper;
using Hooome.Application.Interfaces;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyList;

public class GetCompanyListQueryHandler(ICompanyRepository companyRepo,
    ICompanyImageRepository companyImageRepo,
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
            var companyLogo = await companyImageRepo.GetLogoByCompanyId(company.Id, cancellationToken);

            if (companyLogo is not null)
            {
                company.LogoUrl = await minioService.GetUrl(ImageType.Company, companyLogo.FileName);
            }
        }

        return new CompanyListVm { Companies = companyDtos };
    }
}
