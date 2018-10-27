using System;
using System.Collections.Generic;
using System.Linq;

namespace FzLib.IO.FileProperty.Photo
{
    static class TextHelper
    {

        //private const string Pattern = @"(?<title>^.+)(\r\n)(?<description>^.+)(\r\n)(?<id>^.+)(\r\n)(?<type>^.+)(\r\n)(?<length>^.+)(\s){2}";

        // private static readonly (int id, IFD ifd, string key, string type, string description)[] table;
        public static IReadOnlyList<ExifItem> Items
        {
            get
            {
                if (items == null)
                {
                    List<ExifItem> exifItems = new List<ExifItem>();
                    //  var list = new List<(int id, IFD ifd, string key, string type, string description)>();

                    foreach (var line in Resource.ExifTags.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var infos = line.Split('\t');
                        // list.Add((int.Parse(infos[0]), (IFD)Enum.Parse(typeof(IFD), infos[1]), infos[2], infos[3], infos[4]));
                        var item = new ExifItem()
                        {
                            Id = int.Parse(infos[0]),
                            Ifd = (IFD)Enum.Parse(typeof(IFD), infos[1]),
                            Title = infos[2].Split('.').Last(),
                            Description = infos[4],
                        };
                        if (infos.Length > 5)
                        {
                            item.ChineseTitle = infos[5];
                        }
                        exifItems.Add(item);

                    }

                    items = exifItems.ToList();
                }
                return items;
            }
        }
        private static IReadOnlyList<ExifItem> items;
        // private static readonly Regex Regex = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.Multiline);

        //public static IEnumerable<ExifItem> GetItems()
        //{
        //    return from Match match in Regex.Matches(Resource.Tags)
        //            let title = match.Groups["title"]
        //            let description = match.Groups["description"]
        //            let id = match.Groups["id"]
        //            select new ExifItem
        //            {
        //                Title = title.Value,
        //                Description = description.Value,
        //                Id = int.Parse(id.Value.Substring(2), NumberStyles.HexNumber)
        //            };
        //}
    }
}
