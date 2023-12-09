using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ClientApp.Objects
{
    public class ImageHelper
    {
        public static ImageInfo GetImageInfo(byte[] imageData, int desiredHeight)
        {
            ImageInfo imageInfo = new ImageInfo();
            imageInfo.SetHeight(desiredHeight);

            try
            {
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
                    imageInfo.Width = (int)(decoder.Frames[0].PixelWidth * (desiredHeight / (double)decoder.Frames[0].PixelHeight));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return imageInfo;
        }
    }
}
