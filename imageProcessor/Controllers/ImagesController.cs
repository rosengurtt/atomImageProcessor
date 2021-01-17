using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using imageProcessor.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace imageProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        private IImageStore _imageStore;
        private IImageTransformer _imageTransformer;

        public ImagesController(IImageStore imageStore, IImageTransformer imageTransformer)
        {
            _imageStore = imageStore;
            _imageTransformer = imageTransformer;
        }

        /// <summary>
        /// Retrieves the image that has a file name of "imageName" scaled to a max horizontal sixe of horizontalSizeInPixels
        /// and a max vertical size of verticalSizeInPixels
        /// 
        /// If the "watermark" parameter is included in the request, it adds the watermark to the image
        /// 
        /// Example use:
        /// 
        /// http://localhost:8888/api/images?imageName=01_04_2019_001103&horizontalSizeInPixels=100&watermark=Atom
        /// 
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="horizontalSizeInPixels"></param>
        /// <param name="verticalSizeInPixels"></param>
        /// <param name="watermark"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetImage(string imageName, int horizontalSizeInPixels = 0, int verticalSizeInPixels = 0, string watermark = null)
        {
            var image = await _imageStore.GetImage(imageName, horizontalSizeInPixels, verticalSizeInPixels, watermark);
            if (image == null) return NotFound();
            if (!image.IsCached)
            {
                image = _imageTransformer.Resize(image, horizontalSizeInPixels, verticalSizeInPixels);
                if (!string.IsNullOrEmpty(watermark))
                {
                    image = _imageTransformer.AddWatermark(image, watermark);
                }
                await _imageStore.SaveImage(image);
            }
            return File(image.Bytes, "image/jpeg"); ;
        }
    }
}
