using System;
using System.Linq;

namespace JWT_ODATA_WEB_API.Utils
{
    public static class StringExtensions
    {
        public static string RemoveSnakeCase(this string str)
        {
            return str.Split(new[] {"_"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }
    }
}