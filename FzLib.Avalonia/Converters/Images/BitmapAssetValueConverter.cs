using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace FzLib.Avalonia.Converters;

public class BitmapAssetValueConverter : IValueConverter
{
    public string[] SupportedExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif"];

    public bool ReturnNullIfError { get; set; } = true;

    public virtual Bitmap ConvertToBitmap(string filePath)
    {
        return new Bitmap(filePath);
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        if (value is string filePath)
        {
            var file = new FileInfo(filePath);
            if (!file.Exists)
            {
                return ReturnNullIfError ? null : throw new FileNotFoundException(file.FullName);
            }

            if (SupportedExtensions.Any(ext => file.Extension.Equals(ext, StringComparison.InvariantCultureIgnoreCase)))
            {
                try
                {
                    return ConvertToBitmap(file.FullName);
                }
                catch (Exception ex)
                {
                    return ReturnNullIfError ? null : throw new InvalidOperationException($"无法加载图片文件 {file.FullName}", ex);
                }
            }

            return ReturnNullIfError ? null : throw new ArgumentException($"不支持的文件格式：{file.Extension}");
        }

        return ReturnNullIfError ? null : throw new ArgumentException($"无法将{value.GetType().Name}转换为Bitmap");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}