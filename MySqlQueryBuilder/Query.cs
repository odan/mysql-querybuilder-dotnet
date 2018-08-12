using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace MySqlQueryBuilder
{
    public class Query
    {
        protected MySqlConnection Connection;

        private string _table;

        private readonly List<Field> _fields = new List<Field>();

        private readonly List<WhereCondition> _where = new List<WhereCondition>();

        public Query(MySqlConnection connection)
        {
            Connection = connection;
            _table = "";
        }

        public Query From(string table)
        {
            _table = new Field(table).ToString();

            return this;
        }

        public Query Select(params string[] fields)
        {
            foreach (var field in fields)
            {
                _fields.Add(new Field(field));
            }

            return this;
        }

        public Query Select(params RawExpression[] fields)
        {
            foreach (RawExpression field in fields)
            {
                _fields.Add(new Field(field));
            }

            return this;
        }

        public Query Select(params Field[] fields)
        {
            foreach (Field field in fields)
            {
                _fields.Add(field);
            }

            return this;
        }

        public Query Select(List<object> fields)
        {
            foreach (var field in fields)
            {
                _fields.Add(new Field(field));
            }

            return this;
        }

        public Query ClearSelect()
        {
            _fields.Clear();

            return this;
        }

        public Query Where(string field, string comperator, object value)
        {
            Type type = value.GetType();

            /*
            if (type.IsPrimitive == false ||
               type.FullName != "MySqlQueryBuilder.SubQuery")
            {
                throw new InvalidCastException("Invalid data type: " + type.FullName);
            }*/

            _where.Add(new WhereCondition(field, comperator, value));

            return this;
        }

        public Query WhereColumn(string field, string comperator, string field2)
        {
            return this;
        }

        public RawExpression NewExpr(string raw)
        {
            return new RawExpression(raw);
        }

        public SubQuery ToSubQuery()
        {
            return new SubQuery(Connection, this);
        }

        public QueryFunc Func()
        {
            return new QueryFunc();
        }

        public Statement Execute()
        {
            Statement statement = new Statement(Connection);
            statement.Execute(this.GetSql());

            return statement;
        }

        public string GetSql()
        {
            string sql = "SELECT ";
            string seperator = "";
            foreach (var field in _fields)
            {
                sql += seperator + field;
                seperator = ", ";
            }

            sql += String.Format(" FROM {0}", _table);

            string andSeperator = " WHERE ";
            foreach (WhereCondition where in _where)
            {
                sql += andSeperator + where.ToString();
                andSeperator = " AND ";
            }

            sql += ";";


            return sql;
        }


        public override string ToString()
        {
            return this.GetSql();
        }
    }
}