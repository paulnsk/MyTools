﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCycler.Utils
{
    public static class Utils
    {
        public static bool IsBlank(this string @this) => string.IsNullOrWhiteSpace(@this);
    }
}
