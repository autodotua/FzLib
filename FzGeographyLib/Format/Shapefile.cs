using FzLib.Basic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FzLib.Geography.Format
{
    public static class Shapefile
    {
        public static void ExportEmptyPointShapefile(string folderPath,string name)
        {
            File.WriteAllBytes(GetFileName(folderPath, name, "cpg"), EmptyShapefiles.EmptyPointShapefilesResource.cpg);
            File.WriteAllBytes(GetFileName(folderPath, name, "shp"), EmptyShapefiles.EmptyPointShapefilesResource.shp);
            File.WriteAllBytes(GetFileName(folderPath, name, "shx"), EmptyShapefiles.EmptyPointShapefilesResource.shx);
            File.WriteAllBytes(GetFileName(folderPath, name, "prj"), EmptyShapefiles.EmptyPointShapefilesResource.prj);
            File.WriteAllBytes(GetFileName(folderPath, name, "dbf"), EmptyShapefiles.EmptyPointShapefilesResource.dbf);
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
