using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyTools
{
    public static class DateUtils
    {
        public static int TotalYears(this TimeSpan @this)
        {
            if (@this < TimeSpan.Zero) throw new ArgumentException(nameof(TotalYears) + " can handle positive timespans only");
            return (new DateTime(1, 1, 1) + @this).Year - 1;
        }
    }
}
