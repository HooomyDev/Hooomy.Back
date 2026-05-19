using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Commands.UploadCommentImages;

public class UploadRequestCommentImageCommandHandler(IRequestCommentRepository requestCommentRepo,
    IRequestCommentImageRepository requestCommentImageRepo,
    IMinioService minioService)
    : IRequestHandler<UploadRequestCommentImageCommand>
{
    public async Task Handle(UploadRequestCommentImageCommand request, CancellationToken cancellationToken)
    {
        var existsRequestComment = await requestCommentRepo
            .GetById(request.RequestCommentId, cancellationToken)
            ?? throw new NotFoundException(nameof(RequestCommentImage), request.RequestCommentId);

        foreach(var file in request.Files)
        {
            var imageId = Guid.NewGuid();

            var imageName = await minioService
                .UploadImage(file, Domain.Enums.ImageType.RequestComment, imageId);

            var requestCommentImage = new RequestCommentImage()
            {
                Id = imageId,
                RequestCommentId = request.RequestCommentId,
                FileName = imageName,
                OriginalFileName = file.FileName,
                FileSize = file.Length,
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow
            };

            await requestCommentImageRepo.Create(requestCommentImage, cancellationToken);
        }
    }
}