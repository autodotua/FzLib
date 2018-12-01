using FzLib.Geography.Coordinate;
using System;
using System.Collections.Generic;
using System.Xml;
using static FzLib.Geography.Format.GpxHelper;

namespace FzLib.Geography.Format
{
    public class GpxTrackPointInfo
    {
        public GpxTrackPointInfo() { }
        internal GpxTrackPointInfo(XmlNode xml)
        {
            LoadGpxTrackPointInfoProperties(this, xml);
        }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        public DateTime? Time { get; set; }
        public Dictionary<string, string> OtherProperties { get; internal set; } = new Dictionary<string, string>();

        public MapPoint GetLatLng()
        {
            return new MapPoint(Latitude, Longitude);
        }

    }
}
