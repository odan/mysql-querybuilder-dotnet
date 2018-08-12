using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace MySqlQueryBuilder
{
    public class SubQuery : Query
    {
        private Query _query;

        private string _alias = null;

        public SubQuery(MySqlConnection connection, Query query) : base(connection)
        {
            _query = query;
        }

        public SubQuery Alias(string alias)
        {
            _alias = alias;

            return this;
        }

        public override string ToString()
        {
            string sql = _query.ToString();

            // Remove ;
            sql = sql.Substring(0, sql.Length - 1);

            if (_alias != null)
            {
                // sub query
                sql = String.Format("({0}) AS `{1}`", sql, _alias);
            }
            else
            {
                sql = String.Format("({0})", sql);
            }

            return sql;
        }
    }
}