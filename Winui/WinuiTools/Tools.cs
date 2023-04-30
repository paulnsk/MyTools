using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinuiTools
{
     public static class Tools
     {
         public static string AppTitle => Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly()?.Location ?? "N/A");

     }
}
