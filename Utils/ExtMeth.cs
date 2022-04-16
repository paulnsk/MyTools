using System;
using System.Collections.Generic;
using System.Text;

namespace MyTools
{
    public static class ExtMeth
    {
        public static string ToStringWithInner(this Exception @this)
        {
            if (@this == null) return "";
            var result = "[" + @this.GetType() + "]: " + @this.Message;
            if (@this.InnerException != null) result += "; Inner: " + @this.InnerException.Message;
            return result;
        }


        public static string ShowOnlyStartAndEnd(this string @this, int chars)
        {
            if (chars * 2 >= @this.Length) return @this;
            var begin = @this.Substring(0, chars);
            var end = @this.Substring(@this.Length - chars, chars);
            return $"{begin}<...>{end}";
        }
    }
}
