using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hooome.Application.CQRS.RequestComments.Commands.UploadCommentImages;

public class UploadRequestCommentImageCommand : IRequest
{
    public Guid RequestCommentId { get; set; }
    public List<IFormFile> Files { get; set; } = [];
}
