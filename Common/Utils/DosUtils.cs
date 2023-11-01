using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyTools
{
    public static class DosUtils
    {
        public static void OpenFileRaw(string filepathOrUrl, string arguments = "")
        {
            var p = new Process();
            if (filepathOrUrl.ToLower().StartsWith("http:")) //this prevents "how you want to open this" dialog from appearing
            {
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Verb = "Open";
            }
            p.StartInfo.FileName = filepathOrUrl;
            p.StartInfo.Arguments = arguments;

            p.Start();
            p.WaitForExit();
        }

        public static async Task ProcessDirRecursively(string DirFullPath, string mask, Func<string, Task>? fileAction, Func<string, Task>? dirAction)
        {
            await Task.Run(async () =>
            {
                string[] files = Directory.GetFiles(DirFullPath, mask);
                if (fileAction != null)
                {
                    foreach (string s in files)
                    {
                        await fileAction(s);
                    }
                }

                string[] dirs = Directory.GetDirectories(DirFullPath);
                foreach (string s in dirs)
                {
                    var attr = File.GetAttributes(s);
                    if (attr.HasFlag(FileAttributes.ReparsePoint) || attr.HasFlag(FileAttributes.System)) continue;
                    if (dirAction != null) await dirAction(s);
                    await ProcessDirRecursively(s, mask, fileAction, dirAction);
                }
            });
        }


        public static void StartProcess(string filepathOrUrl, string arguments = "")
        {
            var p = new Process { StartInfo = { FileName = filepathOrUrl, Arguments = arguments, UseShellExecute = true } };
            p.Start();
        }


    }
}
