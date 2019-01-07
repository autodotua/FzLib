using FzLib.Basic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FzLib.Geography.Format
{
    public static class Shapefile
    {
        public static void ExportEmptyPointShapefile(string folderPath, string name)
        {
            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllBytes(GetFileName(folderPath, name, "cpg"), EmptyShapefiles.EmptyShapefilesResource.PointCpg);
            File.WriteAllBytes(GetFileName(folderPath, name, "shp"), EmptyShapefiles.EmptyShapefilesResource.PointShp);
            File.WriteAllBytes(GetFileName(folderPath, name, "shx"), EmptyShapefiles.EmptyShapefilesResource.PointShx);
            File.WriteAllBytes(GetFileName(folderPath, name, "prj"), EmptyShapefiles.EmptyShapefilesResource.PointPrj);
            File.WriteAllBytes(GetFileName(folderPath, name, "dbf"), EmptyShapefiles.EmptyShapefilesResource.PointDbf);
        }
        public static void ExportEmptyMultipointShapefile(string folderPath, string name)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllBytes(GetFileName(folderPath, name, "cpg"), EmptyShapefiles.EmptyShapefilesResource.MultipointCpg);
            File.WriteAllBytes(GetFileName(folderPath, name, "shp"), EmptyShapefiles.EmptyShapefilesResource.MultipointShp);
            File.WriteAllBytes(GetFileName(folderPath, name, "shx"), EmptyShapefiles.EmptyShapefilesResource.MultipointShx);
            File.WriteAllBytes(GetFileName(folderPath, name, "prj"), EmptyShapefiles.EmptyShapefilesResource.MultipointPrj);
            File.WriteAllBytes(GetFileName(folderPath, name, "dbf"), EmptyShapefiles.EmptyShapefilesResource.MultipointDbf);
        }
        public static void ExportEmptyPolylineShapefile(string folderPath, string name)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllBytes(GetFileName(folderPath, name, "cpg"), EmptyShapefiles.EmptyShapefilesResource.PolylineCpg);
            File.WriteAllBytes(GetFileName(folderPath, name, "shp"), EmptyShapefiles.EmptyShapefilesResource.PolylineShp);
            File.WriteAllBytes(GetFileName(folderPath, name, "shx"), EmptyShapefiles.EmptyShapefilesResource.PolylineShx);
            File.WriteAllBytes(GetFileName(folderPath, name, "prj"), EmptyShapefiles.EmptyShapefilesResource.PolylinePrj);
            File.WriteAllBytes(GetFileName(folderPath, name, "dbf"), EmptyShapefiles.EmptyShapefilesResource.PolylineDbf);
        }
        public static void ExportEmptyPolygonShapefile(string folderPath, string name)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            File.WriteAllBytes(GetFileName(folderPath, name, "cpg"), EmptyShapefiles.EmptyShapefilesResource.PolygonCpg);
            File.WriteAllBytes(GetFileName(folderPath, name, "shp"), EmptyShapefiles.EmptyShapefilesResource.PolygonShp);
            File.WriteAllBytes(GetFileName(folderPath, name, "shx"), EmptyShapefiles.EmptyShapefilesResource.PolygonShx);
            File.WriteAllBytes(GetFileName(folderPath, name, "prj"), EmptyShapefiles.EmptyShapefilesResource.PolygonPrj);
            File.WriteAllBytes(GetFileName(folderPath, name, "dbf"), EmptyShapefiles.EmptyShapefilesResource.PolygonDbf);
        }
        private static string GetFileName(string folderPath, string name,string extension)
        {
            if(folderPath.EndsWith("\\"))
            {
                folderPath = folderPath.RemoveEnd("\\", true);
            }
            if(extension.StartsWith("."))
            {
                extension = extension.RemoveStart(".", true);
            }
            if(name.Contains("."))
            {
                name = Path.GetFileNameWithoutExtension(name);
            }
            return folderPath + "\\" + name + "." + extension;
        }
    }
}
