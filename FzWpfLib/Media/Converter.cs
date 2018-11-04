using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMedia = System.Windows.Media;
using WImage = System.Windows.Media.Imaging;
using Draw = System.Drawing;
using System.IO;

namespace FzLib.Media
{
    class Converter
    {
        public static WImage.BitmapImage BitmapToBitmapImage(Draw.Bitmap bitmap)
        {
            //var tempFile=   Path.GetTempFileName();
            //   bitmap.Save(tempFile, ImageFormat.Jpeg);
            //   BitmapImage image = null;
            //   Application.Current.Dispatcher.Invoke(() => image = new BitmapImage(new Uri(tempFile)));
            //   return image;
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, Draw.Imaging.ImageFormat.Jpeg);
                memory.Position = 0;

                var bitmapImage = new WImage.BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = WImage.BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }

        }

        private static Draw.SolidBrush WindowMediaColorToDrawingSolidBrush(WMedia.Color color)
        {
            return new Draw.SolidBrush(Draw.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

    }
}
