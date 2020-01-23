using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.DataStorage.SQLite
{
    public class SQLiteDataType
    {
        private SQLiteDataType(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

        public static SQLiteDataType Integer => new SQLiteDataType("INTEGER");
        public static SQLiteDataType Real => new SQLiteDataType("REAL");
        public static SQLiteDataType Text => new SQLiteDataType("TEXT");
        public static SQLiteDataType Blob => new SQLiteDataType("BLOB");
        public static SQLiteDataType Null => new SQLiteDataType("NULL");

        public static SQLiteDataType Parse(string type)
        {
            switch(type.Trim().ToLower())
            {
                case "integer":
                    return Integer;
                case "real":
                    return Real;
                case "text":
                    return Text;
                case "blob":
                    return Blob;
                case "":
                    return Null;
                default:
                    throw new Exception("未知类型");
            }
        }
    }
}
