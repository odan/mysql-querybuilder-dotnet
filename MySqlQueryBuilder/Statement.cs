using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Threading;
using MySql.Data.MySqlClient;


namespace MySqlQueryBuilder
{
    public class Statement
    {
        private readonly MySqlConnection _connection;

        //private MySqlDataReader reader;
        private MySqlCommand _cmd;

        public Statement(MySqlConnection connection)
        {
            _connection = connection;
        }

        public void Execute(string sql)
        {
            _cmd = new MySqlCommand(sql, _connection);
        }

        public object Fetch()
        {
            return this;
        }

        public T Fetch<T>()
        {
            T row = default(T);

            return row;
        }

        public List<DataRow> FetchAll()
        {
            List<DataRow> rows = new List<DataRow>();

            using (MySqlDataReader reader = _cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    rows.Add(this.DataReaderToDataRow(reader));
                }

                reader.Close();
            }

            return rows;
        }

        public List<T> FetchAll<T>()
        {
            List<T> rows = new List<T>();

            using (MySqlDataReader reader = _cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Convert reader to object and add to list
                    rows.Add(this.DataReaderToObject<T>(reader));
                }

                reader.Close();
            }

            return rows;
        }

        private T DataReaderToObject<T>(IDataRecord dataReader)
        {
            var type = typeof(T);

            // New row object
            var row = (T) Activator.CreateInstance(typeof(T));

            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                var name = ToPascalCase(dataReader.GetName(i));
                var prop = type.GetProperty(name);
                if (prop == null)
                {
                    throw new Exception("Class property not defined: " + name);
                }

                var value = dataReader[i];

                if (value is DBNull)
                {
                    value = null;
                }

                value = Convert.ChangeType(value, prop.PropertyType);

                // Set object property values
                prop.SetValue(row, value);
            }

            return row;
        }

        private DataRow DataReaderToDataRow(IDataRecord reader)
        {
            var row = new DataRow();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                var value = reader[i];

                if (value is DBNull)
                {
                    value = null;
                }

                // Set object property values
                row[name] = value;
            }

            return row;
        }

        // Convert the string to Pascal case.
        private string ToPascalCase(string value)
        {
            value = value.Replace("_", " ").Replace("-", " ").Trim();

            var info = Thread.CurrentThread.CurrentCulture.TextInfo;
            value = info.ToTitleCase(value);
            var parts = value.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            var result = string.Join(string.Empty, parts);

            return result;
        }
    }
}