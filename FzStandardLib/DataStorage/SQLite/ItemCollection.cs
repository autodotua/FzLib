using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace FzLib.DataStorage.SQLite
{
    public class ItemCollection
    {
        public TableHelper Table { get; }
        public ItemCollection(SQLiteConnection dbConnection, string tableName, TableHelper table)
        {
            DbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Table = table ?? throw new ArgumentNullException(nameof(table));
        }

        private SQLiteConnection DbConnection { get; }
        public string TableName { get; }
        public void Add(params string[] values)
        {          
           string sql = "insert into " + TableName + " values (" + string.Join(",", values) + ")";
                     DbConnection.ExecuteNonQuery(sql);

            //StringBuilder str = new StringBuilder("insert into " + TableName + " values (");
            //for(int i=0;i<values.Length;i++)
            //{
            //    if(i!=0)
            //    {
            //        str.Append(",");
            //    }
            //    if(Table.Fields[i].type=="TEXT")
            //    {
            //        str.Append("\"").Append(values[i]).Append("\"");
            //    }
            //    else
            //    {
            //        str.Append(values[i]);

            //    }
            //}
            //DbConnection.ExecuteNonQuery(str.ToString());
        }

        public void Add(params (string column, string value)[] values)
        {
            string sql = "insert into " + TableName + " (" + string.Join(",", values.Select(p => p.column)) + ") values (" + string.Join(",", values.Select(p => p.value)) + ")";
            DbConnection.ExecuteNonQuery(sql);
        }

        public DataTable ToDataTable()
        {
            var sql = "select * from " + TableName;
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, DbConnection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public object[][] ToArray()
        {
            return ToDataTable().ToArray();
        }

        public DataTable Query(QueryParameter para)
        {
            return DbConnection.Query(TableName, para);
        }

        public void Delete(string where)
        {
            DbConnection.ExecuteNonQuery($"delete from {TableName} where {where}");
        }

        public void Update(params (string column, string value)[] values)
        {
            DbConnection.ExecuteNonQuery($"update {TableName} set {string.Join(",", values.Select(p => p.column + "=" + p.value))}");
        }
        public void Update(string where, params (string column, string value)[] values)
        {
            DbConnection.ExecuteNonQuery($"update {TableName} set {string.Join(",", values.Select(p => p.column + "=" + p.value))} where {where}");
        }

  

    }


}

