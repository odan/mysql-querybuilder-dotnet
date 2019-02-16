using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlQueryBuilder
{
    class WhereCondition
    {
        private readonly string _field;

        private readonly string _comperator;

        private readonly object _value;

        private readonly Quoter _quoter = new Quoter();

        public WhereCondition(string field, string comperator, object value)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentException("message", nameof(field));
            }

            if (string.IsNullOrEmpty(comperator))
            {
                throw new ArgumentException("message", nameof(comperator));
            }

            _field = field;
            _comperator = comperator.ToUpper();
            _value = value;
        }

        public override string ToString()
        {
            var field = _field;
            var comperator = _comperator;
            var value = "";

            field = _quoter.QuoteIdentifier(field);

            if (_value == null)
            {
                comperator = "IS";
                if (comperator == "IS NOT" || comperator == "<>" || comperator == "!=")
                {
                    comperator = "IS NOT";
                }
            }

            if (_value is Array)
            {
                value = "(" + _quoter.Quote(_value) + ")";
            }
            else if (_value is RawExpression)
            {
                value = _value.ToString();
            }
            else if (_value is SubQuery)
            {
                value = _value.ToString();
            }
            else
            {
                value = _quoter.Quote(_value);
            }

            return String.Format("{0} {1} {2}", field, comperator, value);
        }
    }
}