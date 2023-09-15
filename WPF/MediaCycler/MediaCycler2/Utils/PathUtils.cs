using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCycler2.Utils
{
    internal static class PathUtils
    {
        public static string ExePath()
        {
            return Process.GetCurrentProcess().MainModule?.FileName ?? "";
            //return Assembly.GetEntryAssembly()?.Location;
        }

        public static string ExeDir()
        {
            return Path.GetDirectoryName(ExePath());
        }

        public static string ExeName()
        {
            return Path.GetFileName(ExePath());
        }

        public static string ExeNameWithoutExt()
        {
            return Path.GetFileNameWithoutExtension(ExePath());
        }


        public static string ExeNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(ExePath());
        }

        public static string FileInExeDir(string fileName)
        {
            return Path.Combine(ExeDir(), fileName);
        }
    }
}
