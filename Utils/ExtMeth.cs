using System;
using System.Collections.Generic;
using System.Text;

namespace MyTools
{
    public static class ExtMeth
    {


        #region Strings

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

        #endregion Strings
    }
}
