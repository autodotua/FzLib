using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.DataStorage.SQLite
{
    public class SQLiteColumn
    {
        public SQLiteColumn(string name,SQLiteDataType type,bool notNull=false,object defaultValue=null)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            NotNull = notNull;
            DefaultValue = defaultValue;
        }

        public SQLiteDataType Type { get; private set; }
        public string Name { get; private set; }
        public object DefaultValue { get; private set; }
        public bool NotNull { get; private set; }
    }
}
