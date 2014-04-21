using System.Drawing;

namespace TerrainAutomator
{
    static class ProcessImage
    {
        public static Image CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmp = new Bitmap(img);
            Bitmap bmpCrop = bmp.Clone(cropArea, bmp.PixelFormat);

            return (Image)(bmpCrop);
        }

        public static Image resizeImage(Image img, Size newSize)
        {
            Bitmap b = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage((Image)b);

            g.DrawImage(img, 0, 0, newSize.Width, newSize.Height);
            g.Dispose();

            return (Image)b;
        }
    }
}
