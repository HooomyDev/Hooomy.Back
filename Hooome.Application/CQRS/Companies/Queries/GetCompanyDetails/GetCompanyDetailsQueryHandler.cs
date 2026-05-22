using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.CQRS.Requests.Queries.GetRequestDetails;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyDetails;

public class GetCompanyDetailsQueryHandler(ICompanyRepository companyRepo,
    ICompanyImageRepository companyImageRepo,
    IRequestReviewRepository requestReviewRepo,
    IMinioService minioService,
    IMapper mapper)
    : IRequestHandler<GetCompanyDetailsQuery, CompanyDetailsVm>
{
    public async Task<CompanyDetailsVm> Handle(GetCompanyDetailsQuery request, CancellationToken cancellationToken)
    {
        var company = await companyRepo.GetById(request.CompanyId, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), request.CompanyId);

        var companyDetails = mapper.Map<CompanyDetailsVm>(company);

        var companyLogo = await companyImageRepo.GetLogoByCompanyId(companyDetails.Id, cancellationToken);

        if (companyLogo is not null)
        {
            companyDetails.LogoUrl = await minioService.GetUrl(ImageType.Company, companyLogo.FileName);
        }

        var reviews = await requestReviewRepo.GetByCompanyId(company.Id, cancellationToken);
        var reviewsDtos = mapper.Map<List<ReviewDto>>(reviews);

        companyDetails.Reviews = reviewsDtos;
        if (reviews is not null)
        {
            companyDetails.AverageRating = reviews.Count != 0
                 ? reviews.Average(r => r.Score)
                 : 0;
        }

        return companyDetails;
    }
}
