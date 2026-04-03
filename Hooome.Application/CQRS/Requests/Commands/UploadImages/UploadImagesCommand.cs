using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hooome.Application.CQRS.Requests.Commands.UploadImages;

public class UploadImagesCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid RequestId { get; set; }
    public List<IFormFile> Files { get; set; } = [];
}
