using Hooome.Domain;
using Microsoft.AspNetCore.Http;

namespace Hooome.Application.Interfaces;

public interface IImageService 
{
    Task SaveImage(IFormFile file);
    Task<List<Image>> SaveImages(List<IFormFile> files, Guid requestId, CancellationToken cancellationToken);
    bool ValidateImage(IFormFile file, out string error);
}
