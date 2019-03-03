using System;

namespace Services.Parser
{
    public static class StringParser
    {
        public static DateTime? DateTimeParse(this string val)
        {
            DateTime var;
            if (!DateTime.TryParse(val, out var))
                return null;

            return var;
        }
        public static bool? BoolParse(this string val)
        {
            bool var;
            if (!bool.TryParse(val, out var))
                return null;

            return var;
        }


    }
}
