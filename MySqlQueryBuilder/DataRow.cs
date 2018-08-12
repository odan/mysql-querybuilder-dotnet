using System;
using System.Collections;

namespace MySqlQueryBuilder
{
    public class DataRow : Hashtable
    {
        public object Get(string field)
        {
            return this[field];
        }

        public object Get(string field, object defaultValue)
        {
            return this.ContainsKey(field) ? this[field] : defaultValue;
        }

        public T Get<T>(string field)
        {
            return this.Get<T>(field, null);
        }

        public T Get<T>(string field, object defaultValue)
        {
            var value = defaultValue;

            if (this.ContainsKey(field))
            {
                value = this[field] == null ? defaultValue : this[field];
            }

            return (T) Convert.ChangeType(value, typeof(T));
        }
    }
}