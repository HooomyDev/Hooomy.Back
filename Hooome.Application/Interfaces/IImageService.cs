using Hooome.Domain;
using Microsoft.AspNetCore.Http;

namespace Hooome.Application.Interfaces;

public interface IImageService 
{
    Task<string> SaveImageAsync(IFormFile file, string entityType, Guid entityId, CancellationToken cancellationToken);
    Task<List<object>> SaveImagesAsync(List<IFormFile> files, string entityType, Guid entityId, CancellationToken cancellationToken);
    bool ValidateImage(IFormFile file, out string error);
}
