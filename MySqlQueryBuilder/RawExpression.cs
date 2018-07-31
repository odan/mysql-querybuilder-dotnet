namespace MySqlQueryBuilder
{
    public class RawExpression
    {
        private string _raw;

        public RawExpression(string raw)
        {
            _raw = raw;
        }

        public string Value()
        {
            return _raw;
        }

        public override string ToString()
        {
            return _raw;
        }
    }
}