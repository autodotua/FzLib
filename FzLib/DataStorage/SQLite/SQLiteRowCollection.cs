using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FzLib.DataStorage.SQLite
{
    public class SQLiteRowCollection
    {
        public SQLiteTableHelper Table { get; }
        public SQLiteRowCollection(SQLiteConnection dbConnection, string tableName, SQLiteTableHelper table)
        {
            DbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Table = table ?? throw new ArgumentNullException(nameof(table));
        }

        private SQLiteConnection DbConnection { get; }
        public string TableName { get; }
        public void Add()
        {
            string sql = "insert into \"" + TableName + "\" values (" + string.Join(",", Enumerable.Repeat("NULL", Table.Columns.Length)) + ")";
            DbConnection.ExecuteNonQuery(sql);
        }
        public int Add(params string[] values)
        {
            string sql = "insert into \"" + TableName + "\" values (" + string.Join(",", values) + ")";
            return DbConnection.ExecuteNonQuery(sql);

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

        public int Add(params (string column, string value)[] values)
        {
            string sql = "insert into \"" + TableName + "\" (" + string.Join(",", values.Select(p => p.column)) + ") values (" + string.Join(",", values.Select(p => p.value)) + ")";
            return DbConnection.ExecuteNonQuery(sql);
        }

        public int Add(params SQLiteData[] datas)
        {
            string columns = string.Join(",", datas.Select(p => p.Column.Name));
            string values = string.Join(",", datas.Select(p => p.ToSqlString()));
            string sql = $"insert into \"{TableName}\"({columns}) values({values})";
            return DbConnection.ExecuteNonQuery(sql);

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



        public int Delete(string where)
        {
            return DbConnection.ExecuteNonQuery($"delete from \"{TableName}\" where {where}");
        }

        public int Update(params (string column, string value)[] values)
        {
            return DbConnection.ExecuteNonQuery($"update \"{TableName}\" set {string.Join(",", values.Select(p => p.column + "=" + p.value))}");
        }
        public int Update(string where, params (string column, string value)[] values)
        {
            return DbConnection.ExecuteNonQuery($"update \"{TableName}\" set {string.Join(",", values.Select(p => p.column + "=" + p.value))} where {where}");
        }



    }


}

