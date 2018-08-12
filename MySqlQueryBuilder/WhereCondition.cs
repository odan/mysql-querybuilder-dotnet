using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlQueryBuilder
{
    class WhereCondition
    {
        private string _field;

        private string _comperator;

        private object _value;

        private Quoter _quoter = new Quoter();

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

            this._field = field;
            this._comperator = comperator.ToUpper();
            this._value = value;
        }

        public override string ToString()
        {
            string field = this._field;
            string comperator = this._comperator;
            string value = "";

            field = _quoter.QuoteIdentifier(field);

            if (this._value == null)
            {
                comperator = "IS";
                if (comperator == "IS NOT" || comperator == "<>" || comperator == "!=")
                {
                    comperator = "IS NOT";
                }
            }

            if (this._value is Array)
            {
                value = "(" + _quoter.Quote(this._value) + ")";
            }
            else if (this._value is RawExpression)
            {
                value = this._value.ToString();
            }
            else if (this._value is SubQuery)
            {
                value = this._value.ToString();
            }
            else
            {
                value = _quoter.Quote(this._value);
            }

            return String.Format("{0} {1} {2}", field, comperator, value);
        }
    }
}