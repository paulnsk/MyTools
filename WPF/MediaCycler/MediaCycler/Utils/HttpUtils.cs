using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaCycler.Utils
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
