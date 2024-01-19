using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.DataStorage.SQLite
{
    public class SQLiteData
    {
        public SQLiteData()
        {

        }
        public SQLiteData(object value,SQLiteColumn column)
        {
            Value = value;
            Column = column;
        }
        public SQLiteData(object value,string columnName,SQLiteDataType type):this(value,new SQLiteColumn(columnName,type))
        {
        }

        public object Value { get; private set; }
        public SQLiteColumn Column { get; private set; }
        public int GetInt32()
        {
            return (int)Value;
        }
        public string ToSqlString()
        {
            switch (Column.Type.Name.ToLower())
            {
                case "integer":
                    return Value.ToString();
                case "real":
                    return Value.ToString();
                case "text":
                    return $"'{ Value.ToString()}'";
                case "blob":
                   throw new Exception("不支持，请在插入处处理");
                case "":
                    return "";
                default:
                    throw new Exception("未知类型");
            }
        }
    }
}
