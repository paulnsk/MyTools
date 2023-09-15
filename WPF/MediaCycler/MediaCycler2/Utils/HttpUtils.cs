using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MediaCycler2.Utils
{
    internal static class HttpUtils
    {

        public static async Task<bool> DownloadFile(this HttpClient @this, string url, string localFilePath)
        {
            try
            {
                using (var stream = await @this.GetStreamAsync(url))
                {
                    using (var fileStream = new FileStream(localFilePath, FileMode.CreateNew))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
