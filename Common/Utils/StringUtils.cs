using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTools
{
    public static class StringUtils
    {
        
        /// <summary>
        /// Displays head and tail of a string which is too long or too secret to be fully shown
        /// </summary>
        /// <param name="this"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string ShowOnlyStartAndEnd(this string @this, int chars)
        {
            if (chars * 2 >= @this.Length) return @this;
            var begin = @this.Substring(0, chars);
            var end = @this.Substring(@this.Length - chars, chars);
            return $"{begin}<...>{end}";
        }



        /// <summary>
        /// Compares 2 strings ignoring case
        /// </summary>
        /// <param name="this"></param>
        /// <param name="otherString"></param>
        /// <returns></returns>
        public static bool SameText(this string @this, string otherString)
        {
            return @this.Equals(otherString, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsBlank(this string? @this) => string.IsNullOrWhiteSpace(@this);


        public static string CommonPrefix(this IEnumerable<string> strings)
        {
            var list = strings?.ToList();
            if (list?.Any() != true) return string.Empty;

            return new string(list?.First()
                .TakeWhile((c, i) => list.All(s => s.Length > i && s[i] == c))
                .ToArray());
        }

        public static string CommonSuffix(this IEnumerable<string> strings)
        {
            var list = strings?.ToList();
            if (list?.Any() != true) return string.Empty;

            return new string(list.First()
                .Reverse()
                .TakeWhile((c, i) => list.All(s => s.Length > i && s.Reverse().ToArray()[i] == c))
                .Reverse()
                .ToArray());
        }

    }
}
