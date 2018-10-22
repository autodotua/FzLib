using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using static FzLib.Data.SQLite.SQLiteHelper;

namespace FzLib.Data.SQLite
{
    public class DatabaseHelper:IDisposable
    {
        private SQLiteConnection DbConnection { get; }
        private FileInfo file;
        public DatabaseHelper(string path)
        {
            file = new FileInfo(path);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            if (!file.Exists)
            {
                SQLiteConnection.CreateFile(path);
            }

            DbConnection = new SQLiteConnection($"Data Source={path};Version=3;");
            DbConnection.Open();
        }

        public bool IsEmpty => file.Length == 0;
        public TableHelper CreateTable(string name, params (string name, string type)[] columns)
        {
            StringBuilder str = new StringBuilder(256);
            str.Append("create table ").Append(name).Append("(");
            //for (int i = 0; i < columns.Length; i++)
            //{
            //    var item = columns[i];
            //    str.Append(item.name).Append(" ").Append(item.type);
            //    if (i < columns.Length - 1)
            //    {
            //        str.Append(",");
            //    }
            //}
            str.Append(string.Join(",", columns.Select(p => p.name + "  " + p.type)));

            str.Append(")");
            DbConnection.ExecuteNonQuery(str.ToString());

            return new TableHelper(DbConnection, name);
        }

        public TableHelper GetTable(string name)
        {
            if (ExistTable(name))
            {
                return new TableHelper(DbConnection, name);
            }
            else
            {
                throw new Exception("表格不存在");
            }
        }

        public bool ExistTable(string name)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT count(*) FROM sqlite_master WHERE type=\"table\" AND name = \"" + name + "\"", DbConnection);
            return (long)command.ExecuteScalar() != 0;
        }

        public string[] GetAllTablesName()
        {
           var dt= DbConnection.Query("select name from sqlite_master where type='table' order by name");

            return dt.Rows.Cast<DataRow>().Select(p => p.ItemArray[0]).Cast<string>().ToArray();
        }

        public void Dispose()
        {
            DbConnection.Dispose();
        }

        public DataTable Union(bool includingRepeatedRows, params QueryParameter[] parameters)
        {
            return DbConnection.Query(string.Join(" union " + (includingRepeatedRows ? "all " : ""), parameters.Select(p=>p.GetSql())));
        }
    }


}

