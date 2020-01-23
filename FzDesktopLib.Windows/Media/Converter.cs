using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMedia = System.Windows.Media;
using WImage = System.Windows.Media.Imaging;
using Draw = System.Drawing;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows;

namespace FzLib.Media
{
    public class Converter
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

        private static Draw.SolidBrush MediaColorToDrawingSolidBrush(WMedia.Color color)
        {
            return new Draw.SolidBrush(Draw.Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public static Draw.Color MediaColorToDrawingColor(WMedia.Color color)
        {
            return Draw.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static WMedia.Color DrawingColorToMeidaColor(Draw.Color color)
        {
            return WMedia.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

       public static Bitmap BitmapSourceToBitmap(BitmapSource source)
        {
            Bitmap bmp = new Bitmap(
              source.PixelWidth,
              source.PixelHeight,
              PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(
              new Rectangle(Draw.Point.Empty, bmp.Size),
              ImageLockMode.WriteOnly,
              PixelFormat.Format32bppPArgb);
            source.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }
    }
}
