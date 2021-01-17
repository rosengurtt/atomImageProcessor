using imageProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace imageProcessor.BusinessLogic
{
    public static class Helpers
    {
        /// <summary>
        /// Used to remove special characters of the watermark when composing the file name for a watermarked image
        /// We add the watermark to the filename so we can later search for an image with that watermark, but we
        /// need to avoid having problems of invalid file names if the water mark contains for example a comma, a
        /// backslash or other special characters
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveSpecialChars(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return Regex.Replace(text, "[^a-z^A-Z^0-9^_]*", "");
        }

        public static string BuildDrinkImageFilename(DrinkImage image)
        {
            return $"{image.ImageName}_hor_{image.HorizontalSize}_ver_{image.VerticalSize}_wat_{Helpers.RemoveSpecialChars(image.Watermark)}.png";
        }

        public static string BuildDrinkImageFilename(string imageName, int horizontalSize, int verticalSize, string watermark, bool replaceZeroSizeWithAsterisk = false)
        {
            var horizontalSizeString = replaceZeroSizeWithAsterisk && horizontalSize == 0 ? "*" : horizontalSize.ToString();
            var verticalSizeString = replaceZeroSizeWithAsterisk && verticalSize == 0 ? "*" : verticalSize.ToString();

            return $"{imageName}_hor_{horizontalSizeString}_ver_{verticalSizeString}_wat_{Helpers.RemoveSpecialChars(watermark)}.png";
        }

    }
}
