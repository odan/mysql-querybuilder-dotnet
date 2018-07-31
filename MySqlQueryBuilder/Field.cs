using System;
using System.Text.RegularExpressions;

namespace MySqlQueryBuilder
{
    public class Field
    {
        private readonly string _field;

        public Field(string field)
        {
            _field = FormatField(field);
        }

        public Field(RawExpression field)
        {
            _field = field.ToString();
        }

        public Field(object field)
        {
            Type theType = field.GetType();
            string value = field.ToString();

             if (theType.FullName != "MySqlQueryBuilder.RawExpression" &&
                theType.FullName != "MySqlQueryBuilder.Field" &&
                theType.FullName != "MySqlQueryBuilder.Query" &&
                theType.FullName != "MySqlQueryBuilder.SubQuery")
            {
                value = this.FormatField(value.ToString());
            }

            _field = value;
        }

        public Field(string field, string alias)
        {
            _field = FormatField(field + " AS " + alias);
            //_alias = alias;
        }

        public string GetField()
        {
            return _field ?? "";
        }

        public override string ToString()
        {
            return _field;
        }

        protected string FormatField(string field)
        {
            string alias = "";
            string pattern = @"^([a-zA-Z0-9_\.]+)\sAS\s(\w+)$";

            // Instantiate the regular expression object.
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

            Match m = r.Match(field);

            if (m.Success)
            {
                field = m.Groups[1].Value;
                alias = m.Groups[2].Value;
            }

            String[] substrings = field.Split('.');
            string parts = "";

            string sep = "";
            foreach (var fieldName in substrings)
            {
                string fieldValue = fieldName == "*" ? fieldName : "`" + fieldName + "`";
                parts += sep + fieldValue;
                sep = ".";
            }

            if (alias.Length > 0)
            {
                parts += " AS `" + alias + "`";
            }

            return parts;
        }
    }
}