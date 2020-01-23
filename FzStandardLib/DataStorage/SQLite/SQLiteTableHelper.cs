using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;

namespace FzLib.DataStorage.SQLite
{
    public class SQLiteTableHelper
    {
        private SQLiteConnection DbConnection { get; }
        public string TableName { get; }

        public SQLiteTableHelper(SQLiteConnection dbConnection, string tableName)
        {
            this.DbConnection = dbConnection;
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            DataTable dt = DbConnection.Query($"Pragma Table_Info({ TableName})");
            //for (int i = 0; i < 6; i++)
            //{
            //    Debug.WriteLine(dt.Rows[0].ItemArray[i].GetType());
            //}
            Columns= dt.Rows.Cast<DataRow>()
                .Select(p =>new SQLiteColumn(p["name"] as string,SQLiteDataType.Parse(p["type"] as string), p["notnull"].Equals(1),p["dflt_value"])).ToArray();
            Rows = new SQLiteRowCollection(dbConnection, tableName,this);
            
        }

        public void Delete()
        {
            DbConnection.ExecuteNonQuery("drop table " + TableName);
        }

        public SQLiteColumn[] Columns { get; }
        public DataTable Union(bool includingRepeatedRows, params QueryParameter[] parameters)
        {
            return DbConnection.Query(string.Join(" union " + (includingRepeatedRows ? "all " : ""), parameters.Select(p => p.GetSql(TableName))));
        }

        public SQLiteRowCollection Rows { get; }

        public void Update(DataTable data)
        {
            var cmd = DbConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM {TableName}";
            var adapter = new SQLiteDataAdapter(cmd);
            SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adapter);
            adapter.Update(data);
        }

        public DataTable Query(QueryParameter para)
        {
            return DbConnection.Query(TableName, para);
        } public DataTable Query(string sql)
        {
            return DbConnection.Query( sql);
        }
    }


}

