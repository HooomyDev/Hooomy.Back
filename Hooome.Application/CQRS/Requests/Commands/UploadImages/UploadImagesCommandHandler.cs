using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Requests.Commands.UploadImages;

public class UploadImagesCommandHandler(IImageService imageService, IHooomeDbContext dbContext)
    : IRequestHandler<UploadImagesCommand>
{
    public async Task Handle(UploadImagesCommand request, CancellationToken cancellationToken)
    {
        var existsRequest = await dbContext.Requests
            .FirstOrDefaultAsync(x => x.UserID == request.UserId && request.RequestId == x.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Request), request.RequestId);

        await imageService.SaveImages(request.Files, existsRequest.Id, cancellationToken);
    }
}
