using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Companies.Commands.UploadLogo;

public class UploadLogoCommandHandler(IHooomeDbContext dbContext, IImageService imageService)
    : IRequestHandler<UploadLogoCommand>
{
    public async Task Handle(UploadLogoCommand request, CancellationToken cancellationToken)
    {
        var existsRequest = await dbContext.Companies
            .FirstOrDefaultAsync(x => request.CompanyId == x.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), request.CompanyId);

        await imageService.SaveImageAsync(request.File, "company", request.CompanyId, cancellationToken);
    }
}
