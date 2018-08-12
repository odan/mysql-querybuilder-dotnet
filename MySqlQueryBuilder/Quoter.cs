using System;
using System.Collections.Generic;
using System.Text;
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

            string result = MySql.Data.MySqlClient.MySqlHelper.EscapeString(value);

            if (result == null)
            {
                return "NULL";
            };

            result = Regex.Replace(result, @"[\x00\b\n\r\t\cZ]", delegate (Match match)
            {
                string v = match.Value;
                switch (v)
                {
                    case "\x00":            // ASCII NUL (0x00) character
                        return "\\0";
                    case "\b":              // BACKSPACE character
                        return "\\b";
                    case "\n":              // NEWLINE (linefeed) character
                        return "\\n";
                    case "\r":              // CARRIAGE RETURN character
                        return "\\r";
                    case "\t":              // TAB
                        return "\\t";
                    case "\u001A":          // Ctrl-Z
                        return "\\Z";
                    default:
                        return "\\" + v;
                }
            });

            return "'" + result + "'";
        }

        public string Quote(bool value)
        {
            return this.Quote(value ? "1" : "0");
        }

        public string Quote(object value)
        {
            if (value == null)
            {
                return "NULL";
            }

            return this.Quote(value.ToString());
        }

        public string QuoteName(string name)
        {
            name = name.Trim();

            string[] separators = { " AS ", " as ", " " };

            foreach (string seperator in separators)
            {
                int position = name.IndexOf(seperator);

                if (position >= 0)
                {
                    return this.QuoteNameWithSeparator(name, seperator, position);
                }
            }

            return this.QuoteIdentifier(name);
        }

        protected string QuoteNameWithSeparator(string name, string seperator, int position)
        {
            string part1 = this.QuoteName(name.Substring(0, position));
            string part2 = this.QuoteIdentifier(name.Substring(position + seperator.Length));

            return String.Format("{0} AS {1}", part1, part2);
        }

        public string QuoteIdentifier(string name)
        {
            name = name.Trim();

            string[] parts = name.Split(".");
            string seperator = "";
            string result = "";

            foreach (string part in parts)
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