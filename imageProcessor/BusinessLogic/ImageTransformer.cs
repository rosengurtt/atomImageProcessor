using imageProcessor.Models;
using SkiaSharp;
using System;
using System.IO;

namespace imageProcessor.BusinessLogic
{
    public class ImageTransformer: IImageTransformer
    {
        /// <summary>
        /// This class takes care of the image processing. It implements a method for resizing images and another for
        /// adding watermarks
        /// 
        /// It uses the library SkiaSharp
        /// 
        /// The caller is agnostic of the library used, it passes and receives images as raw byte arrays. The class
        /// could be refactored to use a different library without affecting the rest of the code.
        /// 
        /// Improvements could be made, like being able to specify the color or the location of the watermark
        /// </summary>
        /// <param name="image"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public DrinkImage Resize(DrinkImage image, int maxWidth, int maxHeight,
        SKFilterQuality quality = SKFilterQuality.Medium)
        {
            using MemoryStream ms = new MemoryStream(image.Bytes);
            using SKBitmap sourceBitmap = SKBitmap.Decode(ms);

            // Calculate height and width. We have to decide what to do when one of the parameters was not passed
            // and was defaulted to 0, or when the parameters passed have a different horizontal/vertical ratio
            // We want to create the smaller file that respects at least one of the 2 sizes provided and that has
            // the same horiz to vertical ratio as the image
            var horiz2vertRatio = sourceBitmap.Width / (double)sourceBitmap.Height;
            maxWidth = maxWidth == 0 ? sourceBitmap.Width : maxWidth;
            maxHeight = maxHeight == 0 ? sourceBitmap.Height : maxHeight;
            var newHorizSize = (int)Math.Round(Math.Min(maxWidth, horiz2vertRatio * maxHeight));
            var newVertSize = (int)Math.Round(Math.Min(maxHeight, maxWidth/ horiz2vertRatio));


            using SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(newHorizSize, newVertSize), quality);
            using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);
            using SKData data = scaledImage.Encode();

            return new DrinkImage
            {
                Bytes = data.ToArray(),
                HorizontalSize = newHorizSize,
                VerticalSize = newVertSize,
                Watermark = image.Watermark,
                ImageName = image.ImageName
            };
        }

        public DrinkImage AddWatermark(DrinkImage imagen, string watermark)
        {

            using MemoryStream ms = new MemoryStream(imagen.Bytes);
            using SKBitmap toBitmap = SKBitmap.Decode(ms);

            using SKCanvas canvas = new SKCanvas(toBitmap);

            using SKTypeface font = SKTypeface.FromFamilyName("Arial");
            using SKPaint brush = new SKPaint
            {
                Typeface = font,
                TextSize = toBitmap.Width * 3 / (watermark.Length * 2),
                IsAntialias = true,
                Color = new SKColor(255, 255, 255, 255)
            };
            canvas.DrawText(watermark, toBitmap.Width / 20, toBitmap.Height/2, brush);

            canvas.Flush();

            using SKImage image = SKImage.FromBitmap(toBitmap);
            using SKData data = image.Encode(SKEncodedImageFormat.Png, 90);

            return new DrinkImage
            {
                Bytes = data.ToArray(),
                VerticalSize = imagen.VerticalSize,
                HorizontalSize = imagen.HorizontalSize,
                Watermark = watermark,
                ImageName = imagen.ImageName
            };
        }
    }
}
