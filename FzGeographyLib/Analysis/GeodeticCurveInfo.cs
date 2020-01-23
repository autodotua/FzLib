using FzLib.Geography;
using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Geography.Analysis
{
    public class GeodeticCurveInfo
    {
        public GeodeticCurveInfo(double length, Angle azimuth, Angle reverseAzimuth)
        {
            Length = length;
            Azimuth = azimuth;
            ReverseAzimuth = reverseAzimuth;
        }

        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get; }
        /// <summary>
        /// 方位角
        /// </summary>
        public Angle Azimuth { get; }
        /// <summary>
        /// 反方位角
        /// </summary>
        public Angle ReverseAzimuth { get; }
    }
}
