namespace SaraCafe.API.Services;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile imageFile);
    bool DeleteImage(string imageUrl);
}

