using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using static FzLib.DataStorage.SQLite.SQLiteHelper;

namespace FzLib.DataStorage.SQLite
{
    public class SQLiteDatabaseHelper : IDisposable
    {
        public SQLiteConnection DbConnection { get; private set; }
        public FileInfo DbFile { get; private set; }
        private SQLiteDatabaseHelper()
        {

        }
        public static SQLiteDatabaseHelper Open(string path)
        {
            SQLiteDatabaseHelper helper = new SQLiteDatabaseHelper();

            helper.DbFile = new FileInfo(path);
            helper.OpenDb(false);
            return helper;
        }

        private void OpenDb(bool ensureExist)
        {
            if (!ensureExist)
            {
                if (!DbFile.Exists)
                {
                    throw new FileNotFoundException("数据库文件不存在");
                }
            }
            DbConnection = new SQLiteConnection($"Data Source={DbFile.FullName};Version=3;");
            DbConnection.Open();
        }

        public static SQLiteDatabaseHelper OpenOrCreate(string path)
        {
            SQLiteDatabaseHelper helper = new SQLiteDatabaseHelper();
            helper.DbFile = new FileInfo(path);
            if (!helper.DbFile.Directory.Exists)
            {
                helper.DbFile.Directory.Create();
            }
            if (!helper.DbFile.Exists)
            {
                SQLiteConnection.CreateFile(path);
            }
            helper.OpenDb(true);
            return helper;
        }

        public bool IsEmpty => DbFile.Length == 0;

        public SQLiteTableHelper CreateTable(string name, string idColumn, params SQLiteColumn[] columns)
        {
            StringBuilder str = new StringBuilder(short.MaxValue);
            str.Append("create table \"").Append(name).Append("\"(");
            //for (int i = 0; i < columns.Length; i++)
            //{
            //    var item = columns[i];
            //    str.Append(item.name).Append(" ").Append(item.type);
            //    if (i < columns.Length - 1)
            //    {
            //        str.Append(",");
            //    }
            //}
            if (idColumn!=null)
            {
                str.Append($"{idColumn} INTEGER PRIMARY KEY AUTOINCREMENT,");
            }
            foreach (var column in columns)
            {
                str.Append(column.Name).Append(" ").Append(column.Type.Name);
                if(column.NotNull)
                {
                    str.Append(" ").Append("not null");
                }
                if(column.DefaultValue!=null)
                {
                    str.Append(" default ").Append(column.DefaultValue.ToString());
                }
                str.Append(",");
            }
            str.Remove(str.Length - 1, 1);

            str.Append(")");
            DbConnection.ExecuteNonQuery(str.ToString());

            return new SQLiteTableHelper(DbConnection, name);
        }

        public SQLiteTableHelper GetTable(string name)
        {
            if (ExistTable(name))
            {
                return new SQLiteTableHelper(DbConnection, name);
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
            var dt = DbConnection.Query("select name from sqlite_master where type='table' order by name");

            return dt.Rows.Cast<DataRow>().Select(p => p.ItemArray[0]).Cast<string>().ToArray();
        }

        public void Dispose()
        {
            DbConnection.Dispose();
        }

        public DataTable Union(bool includingRepeatedRows, params QueryParameter[] parameters)
        {
            return DbConnection.Query(string.Join(" union " + (includingRepeatedRows ? "all " : ""), parameters.Select(p => p.GetSql())));
        }
    }


}

