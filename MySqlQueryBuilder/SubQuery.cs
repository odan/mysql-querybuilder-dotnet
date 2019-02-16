using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace MySqlQueryBuilder
{
    public class SubQuery : Query
    {
        private readonly Query _query;

        private string _alias;

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
            var sql = _query.ToString();

            // Remove ;
            sql = sql.Substring(0, sql.Length - 1);
            sql = _alias != null ? string.Format("({0}) AS `{1}`", sql, _alias) : string.Format("({0})", sql);

            return sql;
        }
    }
}