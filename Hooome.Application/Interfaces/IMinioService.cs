using Hooome.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Hooome.Application.Interfaces;

public interface IMinioService
{
    Task<string> UploadImage(IFormFile file, ImageType type, Guid entityId);
    Task DeleteImage(ImageType type, string imageName);
    string GetUrl(ImageType type, string imageName);
}
