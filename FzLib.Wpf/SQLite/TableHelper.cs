using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;

namespace FzLib.Wpf.SQLite
{
    public class TableHelper
    {
        private SQLiteConnection DbConnection { get; }
        public string TableName { get; }

        public TableHelper(SQLiteConnection dbConnection, string tableName)
        {
            this.DbConnection = dbConnection;
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Items = new ItemCollection(dbConnection, tableName);
        }

        public void Delete()
        {
            DbConnection.ExecuteNonQuery("drop table " + TableName);
        }

        public (long id, string name, string type, bool notNull, bool primaryKey, string defaultValue)[] Fields
        {
            get
            {
                DataTable dt = DbConnection.Query($"Pragma Table_Info({ TableName})");
                for (int i = 0; i < 6; i++)
                {
                    Debug.WriteLine(dt.Rows[0].ItemArray[i].GetType());
                }
                return dt.Rows.Cast<DataRow>()
                    .Select(p => ((long)p.ItemArray[0], p.ItemArray[1] as string, p.ItemArray[2] as string,
                    (long)p.ItemArray[3] == 1, (long)p.ItemArray[5] == 1, p.ItemArray[4] as string)).ToArray();
            }
        }
        public DataTable Union(bool includingRepeatedRows, params QueryParameter[] parameters)
        {
            return DbConnection.Query(string.Join(" union " + (includingRepeatedRows ? "all " : ""), parameters.Select(p => p.GetSql(TableName))));
        }

        public ItemCollection Items { get; }

        public void Update(DataTable data)
        {
            var cmd = DbConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM {TableName}";
            var adapter = new SQLiteDataAdapter(cmd);
            SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adapter);
            adapter.Update(data);
        }
    }


}

