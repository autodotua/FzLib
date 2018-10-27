using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FzLib.IO.FileProperty.Photo
{
    public static class Exif
    {
        public static IEnumerable<ExifItem> Get(string path)
        {
            Bitmap image=null;
            try
            {
                 image = new Bitmap(path);
            }
            catch(Exception ex)
            {
                throw new Exception("读取图片失败", ex);
            }
            try
            {
                return image.PropertyItems.Select(x => x.Convert());
            }
            catch (Exception ex)
            {

                throw new Exception("获取图片属性失败", ex);
            }
            finally
            {
                image.Dispose();
            }
        }

        public static IEnumerable<ExifItem> Get(Stream stream)
        {
            Bitmap image = null;
            try
            {
                image = new Bitmap(stream);
            }
            catch (Exception ex)
            {
                throw new Exception("读取图片失败", ex);
            }
            try
            {
                return image.PropertyItems.Select(x => x.Convert());
            }
            catch (Exception ex)
            {

                throw new Exception("获取图片属性失败", ex);
            }
            finally
            {
                image.Dispose();
            }
        }
    }
}