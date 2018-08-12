using System;
using System.Text.RegularExpressions;

namespace MySqlQueryBuilder
{
    public class Field
    {
        private readonly string _field;

        private Quoter _quoter = new Quoter();

        public Field(string field)
        {
            _field = _quoter.QuoteName(field);
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
                value = _quoter.QuoteName(value.ToString());
            }

            _field = value;
        }

        public Field(string field, string alias)
        {
            _field = _quoter.QuoteName(field + " AS " + alias);
        }

        public string GetField()
        {
            return _field ?? "";
        }

        public override string ToString()
        {
            return _field;
        }
    }
}