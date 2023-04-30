using System;
using System.Collections.Generic;
using System.Text;

namespace MyTools
{
    public static class ErrorUtils
    {
        /// <summary>
        /// Shows Exception.InnerException recursively
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ToStringWithInners(this Exception @this)
        {
            if (@this == null) return "";
            var result = "[" + @this.GetType() + "]: " + @this.Message;
            if (@this.InnerException != null) result += "; Inner: " + @this.InnerException.ToStringWithInners();
            return result;
        }
    }
}
