using System;
using static System.Math;

namespace FzLib.Geography.Coordinate.Convert
{
    public static class TileConverter
    {
        public static (int x, int y) GetSlippyMapTile(double lat, double lng, int level)
        {
            var x = (lng + 180) / 360;
            int tileX = (int)(x * Pow(2, level));

            var lat_rad = lat * PI / 180;
            var y = (0.5 - Log(Tan(lat_rad) + 1 / Cos(lat_rad)) / (2 * PI));
            int tileY = (int)(y * Pow(2, level));
            return (tileX, tileY);
        }

        /*
   * 某一瓦片等级下瓦片地图X轴(Y轴)上的瓦片数目
   */
        private static double _getMapSize(double level)
        {
            return Pow(2, level);
        }

        /*
         * 分辨率，表示水平方向上一个像素点代表的真实距离(m)
         */
        private static double getResolution(double latitude, int level)
        {
            var resolution = 6378137.0 * 2 * PI * Cos(latitude) / 256 / _getMapSize(level);
            return resolution;
        }

        private static int _lngToTileX(double longitude, int level)
        {
            var x = (longitude + 180) / 360;
            var tileX = (int)(x * _getMapSize(level));
            return tileX;
        }

        private static int _latToTileY(double latitude, int level)
        {
            var lat_rad = latitude * PI / 180;
            var y = (1 - Log(Tan(lat_rad) + 1 / Cos(lat_rad)) / PI) / 2;
            var tileY = (int)(y * _getMapSize(level));

            // 代替性算法,使用了一些三角变化，其实完全等价
            //var sinLatitude = Sin(latitude * PI / 180);
            //var y = 0.5 - Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * PI);
            //var tileY = (int)(y * _getMapSize(level));

            return tileY;
        }

        /*
         * 从经纬度获取某一级别瓦片坐标编号
         */
        public static (int x, int y) GeoPointToTile(double latitude, double longitude, int level)
        {
            var tileX = _lngToTileX(longitude, level);
            var tileY = _latToTileY(latitude, level);
            return (tileX, tileY);

        }

        private static int _lngToPixelX(double longitude, int level)
        {
            var x = (longitude + 180) / 360;
            var pixelX = (int)(x * _getMapSize(level) * 256 % 256);

            return pixelX;
        }

        private static int _latToPixelY(double latitude, int level)
        {
            var sinLatitude = Sin(latitude * PI / 180);
            var y = 0.5 - Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * PI);
            var pixelY = (int)(y * _getMapSize(level) * 256 % 256);

            return pixelY;
        }

        /*
         * 从经纬度获取点在某一级别瓦片中的像素坐标
         */
        public static (int x, int y) GeoPointToPixel(double latitude, double longitude, int level)
        {
            var pixelX = _lngToPixelX(longitude, level);
            var pixelY = _latToPixelY(latitude, level);

            return (pixelX, pixelY);
        }

        private static double _pixelXTolng(int pixelX, int tileX, int level)
        {
            var pixelXToTileAddition = pixelX / 256.0;
            var lngitude = (tileX + pixelXToTileAddition) / _getMapSize(level) * 360 - 180;

            return lngitude;
        }

        private static double _pixelYToLat(int pixelY, int tileY, int level)
        {
            var pixelYToTileAddition = pixelY / 256.0;
            var latitude = Atan(Sinh(PI * (1 - 2 * (tileY + pixelYToTileAddition) / _getMapSize(level)))) * 180.0 / PI;

            return latitude;
        }

        /*
         * 从某一瓦片的某一像素点到经纬度
         */
        public static (double lat, double lng) PixelToGeoPoint(int pixelX, int pixelY, int tileX, int tileY, int level)
        {
            var lng = _pixelXTolng(pixelX, tileX, level);
            var lat = _pixelYToLat(pixelY, tileY, level);

            return (lat, lng);
        }

    }
}
