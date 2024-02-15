namespace SpiralWaveHub.Services
{
    public interface IImageService
    {
        Task<(bool isUploaded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath);
        void DeleteImage(string imagePath, string thumbNailPath);

        MemoryStream GetImageInStream(string path);
    }
}
