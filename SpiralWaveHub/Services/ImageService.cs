using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SpiralWaveHub.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
       private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;

       public ImageService(IWebHostEnvironment webHostEnvironment)
       {
            _webHostEnvironment = webHostEnvironment;
        }

        public void DeleteImage(string imagePath, string thumbNailPath )
        {
            //get the old image path in the application to delete it
            var oldImagePath = $"{_webHostEnvironment.WebRootPath}{imagePath}";
            var oldThumbPath = $"{_webHostEnvironment.WebRootPath}{thumbNailPath}";
            //delete the old image
            if (File.Exists(oldImagePath))
                File.Delete(oldImagePath);
            //delete the thumbnail
            if (File.Exists(oldThumbPath))
                File.Delete(oldThumbPath);
        }

        public MemoryStream GetImageInStream(string imagePath)
        {
            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}{imagePath}");

            byte[] imageData = File.ReadAllBytes(path);

            // Create a MemoryStream to store the image data
            MemoryStream picStream = new MemoryStream(imageData);

            return picStream;
        }

        public async Task<(bool isUploaded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath)
        {
            var extension = Path.GetExtension(image.FileName);

            if (!_allowedExtensions.Contains(extension))
                return (isUploaded: false, errorMessage: Errors.NotAllowedExtension);

            if (image.Length > _maxAllowedSize)
                return (isUploaded: false, errorMessage: Errors.MaxSize);

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}{folderPath}", imageName);
            using var stream = File.Create(path);
            await image.CopyToAsync(stream);
            stream.Dispose();

            //handle thumbnail

            var thumbPath = Path.Combine($"{_webHostEnvironment.WebRootPath}{folderPath}/thumb", imageName);

            using var loadedImage = Image.Load(image.OpenReadStream());
            var ratio = (float)loadedImage.Width / 200;
            var height = loadedImage.Height / ratio;
            loadedImage.Mutate(i => i.Resize(width: 200, height: (int)height));
            loadedImage.Save(thumbPath);

            return (isUploaded: true, errorMessage: null);
        }
    }
}
