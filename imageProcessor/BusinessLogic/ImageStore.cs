using imageProcessor.Models;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace imageProcessor.BusinessLogic
{
    public class ImageStore : IImageStore
    {
        private string _processedImagesFolder;
        private string _rawImagesFolder;

        public ImageStore(IOptions<AppConfig> appConfiguration)
        {
            _rawImagesFolder = appConfiguration.Value.RawImagesFolder;
            _processedImagesFolder = appConfiguration.Value.ProcessedImagesFolder;
        }
        /// <summary>
        /// This implementation of the store saves the files to the file system
        /// 
        /// It assumes there is a folder in the application directory that has the raw image files. The path to this folder
        /// is defined in the appSettings file. It uses another folder to use as the cache, again with the path to this folder
        /// provided in appSettings
        /// 
        /// Tries to return the image with the size and watermark passed in the parameters by looking first in the cache, and
        /// then in the raw images folder
        /// 
        /// If it can't find in the cache the right image it returns a raw image with whatever size and no watermark
        /// 
        /// It sets a flag to indicate if the file comes from the cache or the raw images folder
        /// 
        /// If there isn't a file with such name in the repository, it returns null
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="verticalSize"></param>
        /// <param name="horizontalSize"></param>
        /// <param name="watermark"></param>
        /// <returns></returns>
        public async Task<DrinkImage> GetImage(string imageName, int horizontalSize, int verticalSize, string watermark)
        {
            var cachedImage = await GetImageFromCache(imageName, horizontalSize, verticalSize, watermark);
            if (cachedImage != null) return cachedImage;
            string filePath = Path.Combine(_rawImagesFolder, imageName);
            if (!filePath.ToLower().EndsWith(".png")) filePath += ".png";
            if (File.Exists(filePath))
                return new DrinkImage
                {
                    Bytes = await File.ReadAllBytesAsync(filePath),
                    HorizontalSize = 0,
                    VerticalSize = 0,
                    Watermark = "",
                    ImageName = imageName,
                    IsCached = false
                };
            else
                return null;
        }
        /// <summary>
        /// Tries to retrieve an image from the cache. If it can't find an image with the size and watermark requested
        /// returns null
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="verticalSize"></param>
        /// <param name="horizontalSize"></param>
        /// <param name="watermark"></param>
        /// <returns></returns>
        public async Task<DrinkImage> GetImageFromCache(string imageName, int horizontalSize, int verticalSize, string watermark)
        {
            string fileName = Helpers.BuildDrinkImageFilename(imageName, horizontalSize, verticalSize, watermark, true);
            string[] files = null;
            await Task.Run(() =>
            {
                files = Directory.GetFiles(_processedImagesFolder, fileName);
            });
            if (files != null && files.Length == 1)
            {
                return new DrinkImage
                {
                    Bytes = await File.ReadAllBytesAsync(files[0]),
                    HorizontalSize = horizontalSize,
                    VerticalSize = verticalSize,
                    Watermark = watermark,
                    ImageName = imageName,
                    IsCached = true
                };
            }
            else
                return null;
        }

        public async Task SaveImage(DrinkImage image)
        {
            string fileName = Helpers.BuildDrinkImageFilename(image);
            string filePath = Path.Combine(_processedImagesFolder, fileName);
            await File.WriteAllBytesAsync(filePath, image.Bytes);
        }
    }
}
