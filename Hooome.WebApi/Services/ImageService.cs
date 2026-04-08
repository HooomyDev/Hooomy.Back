using Hooome.Application.Interfaces;
using Hooome.Domain;

namespace Hooome.WebApi.Services;

public class ImageService(IWebHostEnvironment environment, IHooomeDbContext dbContext) : IImageService
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
    private const long MaxFileSize = 10 * 1024 * 1024;

    public async Task<string> SaveImageAsync(IFormFile file, string entityType, Guid entityId, CancellationToken cancellationToken)
    {
        if (!ValidateImage(file, out var error))
            throw new ArgumentException(error);

        var extension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{entityType}_{entityId}_{Guid.NewGuid()}{extension}";
        var uploadPath = Path.Combine(environment.WebRootPath, "uploads", entityType.ToLower());

        Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, uniqueFileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        if (entityType == "company")
        {
            var image = new CompanyImage
            {
                Id = Guid.NewGuid(),
                FileName = uniqueFileName,
                FilePath = $"/uploads/{entityType.ToLower()}/{uniqueFileName}",
                FileSize = file.Length,
                CompanyId = entityId,
                IsMain = true
            };

            await dbContext.CompanyImages.AddAsync(image, cancellationToken);
        }
        else if (entityType == "request")
        {
            var image = new RequestImage
            {
                Id = Guid.NewGuid(),
                FileName = uniqueFileName,
                FilePath = $"/uploads/{entityType.ToLower()}/{uniqueFileName}",
                FileSize = file.Length,
                RequestId = entityId,
                IsMain = true
            };

            await dbContext.RequestImages.AddAsync(image, cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return $"/uploads/{entityType.ToLower()}/{uniqueFileName}";
    }

    public async Task<List<object>> SaveImagesAsync(List<IFormFile> files, string entityType, Guid entityId, CancellationToken cancellationToken)
    {
        var savedImages = new List<object>();
        var uploadsPath = Path.Combine(environment.WebRootPath, "uploads", entityType.ToLower());

        Directory.CreateDirectory(uploadsPath);

        for (var i = 0; i < files.Count; i++)
        {
            var file = files[i];

            if (!ValidateImage(file, out var error))
                continue;

            var fileName = $"{entityType}_{entityId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            if (entityType.Equals("company", StringComparison.CurrentCultureIgnoreCase))
            {
                var image = new CompanyImage
                {
                    Id = Guid.NewGuid(),
                    FileName = file.FileName,
                    FilePath = $"/uploads/{entityType.ToLower()}/{fileName}",
                    FileSize = file.Length,
                    CompanyId = entityId,
                    IsMain = i == 0
                };

                await dbContext.CompanyImages.AddAsync(image, cancellationToken);
                savedImages.Add(image);
            }
            else if (entityType.Equals("request", StringComparison.CurrentCultureIgnoreCase))
            {
                var image = new RequestImage
                {
                    Id = Guid.NewGuid(),
                    FileName = file.FileName,
                    FilePath = $"/uploads/{entityType.ToLower()}/{fileName}",
                    FileSize = file.Length,
                    RequestId = entityId,
                    IsMain = i == 0
                };

                await dbContext.RequestImages.AddAsync(image, cancellationToken);
                savedImages.Add(image);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return savedImages;
    }

    public bool ValidateImage(IFormFile file, out string error)
    {
        error = string.Empty;

        if (file is null || file.Length == 0)
        {
            error = "File not selected";
            return false;
        }

        var extension = Path.GetExtension(file.FileName);

        if (!_allowedExtensions.Contains(extension))
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
