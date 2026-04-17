using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.UploadImages;

public class UploadImagesCommandHandler(IImageService imageService, IRequestRepository requestRepo)
    : IRequestHandler<UploadImagesCommand>
{
    public async Task Handle(UploadImagesCommand request, CancellationToken cancellationToken)
    {
        var existsRequest = await requestRepo
            .GetByIdAndUserId(request.RequestId, request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Request), request.RequestId);

        await imageService.SaveImagesAsync(request.Files, "request", request.RequestId, cancellationToken);
    }
}
