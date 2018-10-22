using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using static FzLib.Data.SQLite.SQLiteHelper;

namespace FzLib.Data.SQLite
{
    public static class SQLiteHelper
    {
        public static void ExecuteNonQuery(this SQLiteConnection dbConnection, string commandString)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }

            using (SQLiteCommand command = new SQLiteCommand(commandString, dbConnection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static object[][] ToArray(this DataTable table)
        {
            return table.Rows.Cast<DataRow>().Select(p => p.ItemArray).ToArray();
        }

        public static DataTable Query(this SQLiteConnection dbConnection,string tableName, QueryParameter para)
        {
            return dbConnection.Query(para.GetSql(tableName));
        }
        public static DataTable Query(this SQLiteConnection dbConnection,string sql)
        {
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, dbConnection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }


}

