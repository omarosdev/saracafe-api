namespace SaraCafe.API.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;
    private const string ImagesFolder = "uploads/images";

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveImageAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            throw new ArgumentException("Image file is required");
        }

        // Validate file extension
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new ArgumentException("Invalid file extension. Allowed: jpg, jpeg, png, gif, webp");
        }

        // Create uploads directory if it doesn't exist
        var basePath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }
        
        var uploadsPath = Path.Combine(basePath, ImagesFolder);
        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }

        // Generate unique filename
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsPath, uniqueFileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        // Return relative URL
        return $"/{ImagesFolder}/{uniqueFileName}";
    }

    public bool DeleteImage(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            return false;
        }

        try
        {
            var basePath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
            var filePath = Path.Combine(basePath, imageUrl.TrimStart('/'));
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
}

