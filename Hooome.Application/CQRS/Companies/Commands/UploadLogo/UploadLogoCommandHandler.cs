using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.UploadLogo;

public class UploadLogoCommandHandler(ICompanyRepository companyRepo,
    ICompanyImageRepository companyImageRepo,
    IMinioService minioService)
    : IRequestHandler<UploadLogoCommand>
{
    public async Task Handle(UploadLogoCommand request, CancellationToken cancellationToken)
    {
        var existsCompany = await companyRepo.GetById(request.CompanyId, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), request.CompanyId);

        var newLogoId = Guid.NewGuid();

        var logoName = await minioService.UploadImage(request.File, ImageType.Company, newLogoId);

        var newLogo = new CompanyImage()
        {
            Id = newLogoId,
            CompanyId = request.CompanyId,
            FileName = logoName,
            OriginalFileName = request.File.Name,
            FileSize = request.File.Length,
            ContentType = request.File.ContentType,
            UploadedAt = DateTime.UtcNow
        };

        await companyImageRepo.Create(newLogo, cancellationToken);
    }
}
