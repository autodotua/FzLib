using WMedia = System.Windows.Media;
using Draw = System.Drawing;
using System.IO;

using System.Drawing;

using System.Windows.Media.Imaging;

using System.Drawing.Imaging;
using System.Windows;

namespace FzLib.WPF
{
    public static class ColorExtension
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static SolidBrush ToSolidBrush(this WMedia.Color color)
        {
            return new SolidBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public static Color ToDrawingColor(this WMedia.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static WMedia.Color ToMeidaColor(this Color color)
        {
            return WMedia.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Bitmap ToBitmap(this BitmapSource source)
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