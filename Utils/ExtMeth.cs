using System;
using System.Collections.Generic;
using System.Text;

namespace MyTools
{
    public static class ExtMeth
    {
        public static string ToStringWithInner(this Exception @this)
        {
            var result = "[" + @this.GetType() + "]: " + @this.Message;
            if (@this.InnerException != null) result += "; Inner: " + @this.InnerException.Message;
            return result;
        }
    }
}
