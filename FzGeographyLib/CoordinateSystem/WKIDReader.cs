﻿using GeoAPI.CoordinateSystems;
using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FzLib.Geography.CoordinateSystem
{
   internal class SRIDReader
    {
        private static readonly Lazy<ICoordinateSystemFactory> CoordinateSystemFactory =
            new Lazy<ICoordinateSystemFactory>(() => new CoordinateSystemFactory());

        public struct WktString
        {
            /// <summary>
            /// Well-known ID
            /// </summary>
            public int WktId;
            /// <summary>
            /// Well-known Text
            /// </summary>
            public string Wkt;
        }

        /// <summary>
        /// Enumerates all SRID's in the SRID.csv file.
        /// </summary>
        /// <returns>Enumerator</returns>
        public static IEnumerable<WktString> GetSrids(string filename = null)
        {
            //var stream = string.IsNullOrWhiteSpace(filename)
            //    ? Assembly.GetExecutingAssembly().GetManifestResourceStream("ProjNET.Tests.SRID.csv")
            //    : File.OpenRead(filename);

            using (StringReader reader = new StringReader(Resource.SRID))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    var split = line.IndexOf(';');
                    if (split <= -1) continue;

                    var wkt = new WktString
                    {
                        WktId = int.Parse(line.Substring(0, split)),
                        Wkt = line.Substring(split + 1)
                    };
                    yield return wkt;
                }
            }
        }

        /// <summary>
        /// Gets a coordinate system from the SRID.csv file
        /// </summary>
        /// <param name="id">EPSG ID</param>
        /// <param name="file">(optional) path to CSV File with WKT definitions.</param>
        /// <returns>Coordinate system, or <value>null</value> if no entry with <paramref name="id"/> was not found.</returns>
        internal static ICoordinateSystem GetCSbyID(int id, string file = null)
        {
            //ICoordinateSystemFactory factory = new CoordinateSystemFactory();
            foreach (var wkt in GetSrids(file))
                if (wkt.WktId == id)
                    return CoordinateSystemFactory.Value.CreateFromWkt(wkt.Wkt);
            return null;
        }
    }
}
