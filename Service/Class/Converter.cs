using System.Text.RegularExpressions;

namespace Services.Class
{
    public static class StringConverter
    {
        public static decimal decimalParse(string input)
        {
            return decimal.Parse(Regex.Match(input, @"-?\d{1,3}(,\d{3})*(\.\d+)?").Value);
        }
    }
}
