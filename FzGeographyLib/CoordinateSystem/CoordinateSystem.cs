using GeoAPI.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Geography.CoordinateSystem
{
    class CoordinateSystemExtend
    {
        public static ICoordinateSystem Get(int wkid)
        {
            if (cache.ContainsKey(wkid))
            {
                return cache[wkid];
            }
            ICoordinateSystem result = SRIDReader.GetCSbyID(wkid);
            cache.Add(wkid, result);
            return result;
        }

        protected static readonly Dictionary<int, ICoordinateSystem> cache = new Dictionary<int, ICoordinateSystem>();
    }
}
