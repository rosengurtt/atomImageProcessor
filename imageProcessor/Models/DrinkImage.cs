namespace imageProcessor.Models
{
    public class DrinkImage
    {
        public string ImageName { get; set; }
        public byte[] Bytes { get; set; }
        public int VerticalSize { get; set; }
        public int HorizontalSize { get; set; }
        public string Watermark { get; set; }
        public bool IsCached { get; set; }
    }
}
