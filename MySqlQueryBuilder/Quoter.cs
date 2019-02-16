using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MySqlQueryBuilder
{
    public class Quoter
    {
        public string Quote(string value)
        {
            if (value == null)
            {
                return "NULL";
            }

            var result = MySql.Data.MySqlClient.MySqlHelper.EscapeString(value);

            if (result == null)
            {
                return "NULL";
            }

            result = Regex.Replace(result, @"[\x00\b\n\r\t\cZ]", delegate(Match match)
            {
                var v = match.Value;
                switch (v)
                {
                    case "\x00": // ASCII NUL (0x00) character
                        return "\\0";
                    case "\b": // BACKSPACE character
                        return "\\b";
                    case "\n": // NEWLINE (linefeed) character
                        return "\\n";
                    case "\r": // CARRIAGE RETURN character
                        return "\\r";
                    case "\t": // TAB
                        return "\\t";
                    case "\u001A": // Ctrl-Z
                        return "\\Z";
                    default:
                        return "\\" + v;
                }
            });

            return "'" + result + "'";
        }

        public string QuoteArray(object[] values)
        {
            string result = "";
            string seperator = "";

            foreach (var value in values)
            {
                result += seperator + this.Quote(value);
                seperator = ", ";
            }

            return result;
        }

        public string Quote(bool value)
        {
            return this.Quote(value ? "1" : "0");
        }

        public string Quote(float value)
        {
            return this.Quote(value.ToString(CultureInfo.InvariantCulture).Replace(",", "."));
        }

        public string Quote(double value)
        {
            return this.Quote(value.ToString(CultureInfo.InvariantCulture).Replace(",", "."));
        }

        public string Quote(object value)
        {
            switch (value)
            {
                case null:
                    return "NULL";
                case float f:
                    return this.Quote(f);
                case double d:
                    return this.Quote(d);
                case bool b:
                    return this.Quote(b);
                case Array _:
                    return this.QuoteArray(value as object[]);
            }

            return this.Quote(value.ToString());
        }

        public string QuoteName(string name)
        {
            name = name.Trim();

            string[] separators = {" AS ", " as ", " "};

            foreach (string seperator in separators)
            {
                int position = name.IndexOf(seperator, StringComparison.Ordinal);

                if (position >= 0)
                {
                    return this.QuoteNameWithSeparator(name, seperator, position);
                }
            }

            return this.QuoteIdentifier(name);
        }

        public string QuoteNameWithSeparator(string name, string seperator, int position)
        {
            var part1 = this.QuoteName(name.Substring(0, position));
            var part2 = this.QuoteIdentifier(name.Substring(position + seperator.Length));

            return string.Format("{0} AS {1}", part1, part2);
        }

        public string QuoteIdentifier(string name)
        {
            name = name.Trim();

            var parts = name.Split(".");
            var seperator = "";
            var result = "";

            foreach (var part in parts)
            {
                if (part == "*")
                {
                    result += seperator + part;
                }
                else
                {
                    result += seperator + "`" + part.Replace("`", "``") + "`";
                }

                seperator = ".";
            }

            return result;
        }
    }
}