using imageProcessor.Models;
using SkiaSharp;

namespace imageProcessor.BusinessLogic
{
    public interface IImageTransformer
    {
        public DrinkImage Resize(DrinkImage image, int maxWidth, int maxHeight, SKFilterQuality quality = SKFilterQuality.Medium);
        public DrinkImage AddWatermark(DrinkImage imagen, string watermark);
    }
}
