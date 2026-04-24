using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.UploadImages;

public class UploadImagesCommandHandler(IRequestRepository requestRepo,
    IRequestImageRepository requestImageRepo, 
    IMinioService minioService)
    : IRequestHandler<UploadImagesCommand>
{
    public async Task Handle(UploadImagesCommand request, CancellationToken cancellationToken)
    {
        var existsRequest = await requestRepo
            .GetByIdAndUserId(request.RequestId, request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Request), request.RequestId);

        foreach(var file in request.Files)
        {
            var imageId = Guid.NewGuid();

            var imageName = await minioService
                .UploadImage(file, Domain.Enums.ImageType.Request, imageId);

            var requestImage = new RequestImage()
            {
                Id = imageId,
                RequestId = request.RequestId,
                FileName = imageName,
                OriginalFileName = file.FileName,
                FileSize = file.Length,
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow
            };

            await requestImageRepo.Create(requestImage, cancellationToken);
        }
    }
}
