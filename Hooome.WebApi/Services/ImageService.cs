using Hooome.Application.Interfaces;
using Hooome.Domain;

namespace Hooome.WebApi.Services;

public class ImageService(IWebHostEnvironment environment, IHooomeDbContext dbContext) : IImageService
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
    private const long MaxFileSize = 10 * 1024 * 1024;

    public async Task SaveImage(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var uploadPath = Path.Combine(environment.WebRootPath, "uploads", "images");

        Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, uniqueFileName);

        using var stream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream);
    }

    public async Task<List<Image>> SaveImages(List<IFormFile> files, Guid requestId, CancellationToken cancellationToken)
    {
        {
            var savedImages = new List<Image>();
            var uploadsPath = Path.Combine(environment.WebRootPath, "uploads", "requests");

            Directory.CreateDirectory(uploadsPath);

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
      
                await file.CopyToAsync(stream, cancellationToken);

                var image = new Image
                {
                    Id = Guid.NewGuid(),
                    FileName = fileName,
                    FilePath = $"/uploads/requests/{fileName}",
                    FileSize = file.Length,
                    RequestId = requestId,
                    IsMain = savedImages.Count == 0
                };

                dbContext.Images.Add(image);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return savedImages;
        }
    }

    public bool ValidateImage(IFormFile file, out string error)
    {
        error = string.Empty;

        if(file is null || file.Length == 0)
        {
            error = "File not selected";
            return false;
        }

        var extension = Path.GetExtension(file.FileName);

        if(!_allowedExtensions.Contains(extension))
        {
            error = $"Unsupported file format. Allowed formats: {string.Join(", ", _allowedExtensions)}";
            return false;
        }

        if (file.Length > MaxFileSize)
        {
            error = $"The file is too large. Maximum size: {MaxFileSize / 1024 / 1024}MB";
            return false;
        }

        return true;
    }
}
