using System.Linq;
using System.Text;
using System.Collections.Generic;
using System;

namespace FzLib.Data.SQLite
{
    public class QueryParameter
    {
        public string From { get; set; }
        public string Select { get; set; } = "*";
        public string Where { get; set; } = null;
        public string Like { get; set; } = null;
        public bool CaseSensitive { get; set; } = false;
        public int Limit { get; set; } = 0;
        public int Offset { get; set; } = 0;
        public int OrderBy { get; set; } = 0;
        public IEnumerable<string> OrderByColumns { get; set; }
        public bool Distinct { get; set; }

        public string GetSql(string tableName)
        {
            From = tableName;
            return GetSql();
        }

        public string GetSql()
        {
            if(From==null || From=="")
            {
                throw new ArgumentNullException("From为空");
            }
            StringBuilder sql = new StringBuilder("select ");
            if(Distinct)
            {
                sql.Append("Distinct ");
            }
            if(Select==null || Select=="")
            {
                sql.Append("*");
            }
            else
            {
                sql.Append(Select);
            }
            sql.Append(" from ").Append(From);
            if (Where != null && Where != "")
            {
                sql.Append(" where " + Where);
                if (Like != null && Like != "")
                {
                    sql.Append($" {(CaseSensitive ? "glob" : "like")} { Like}");
                }
            }

            if (Limit > 0)
            {
                sql.Append($" limit { Limit.ToString()}");
                if (Offset > 0)
                {
                    sql.Append($" offset { Offset.ToString()}");
                }
            }

            if (OrderBy != 0 && OrderByColumns != null && OrderByColumns.Count() > 0)
            {
                sql.Append(" order by ").Append(string.Join(",", OrderByColumns)).Append(" ").Append(OrderBy > 0 ? "asc" : "desc");
            }
            return sql.ToString();
        }
    }


}

