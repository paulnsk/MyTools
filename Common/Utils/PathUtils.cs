using System;
using System.Diagnostics;
using System.IO;

namespace MyTools
{
    public static class PathUtils
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

        public static void EnsureDir(string filePath)
        {
            var dirPath = Path.GetDirectoryName(filePath);
            if (dirPath.IsBlank()) throw new Exception($"{nameof(dirPath)} empty in {nameof(EnsureDir)}");
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath!);
            if (!Directory.Exists(dirPath)) throw new Exception($" Unable to create directory {dirPath}");
        }

    }
}
