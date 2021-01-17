using imageProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imageProcessor.BusinessLogic
{ 
    /// <summary>
    /// The interface for the access of the stored images
    /// 
    /// It implements one method used to retrieve images by providing name, size and watermark
    /// 
    /// The method tries to return the image with the size and watermark requested,
    /// but it may return an image with a different size and no watermark
    /// </summary>
    public interface IImageStore
    {
        public Task<DrinkImage> GetImage(string imageName, int horizontalSize, int verticalSize, string watermark);
        public Task SaveImage(DrinkImage image);
    }
}
