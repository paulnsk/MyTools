using System;
using System.IO;

namespace MediaCycler.Utils
{
    public static class UrlUtils
    {
        public static bool IsLocalUrl(this string url)
        {
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
            {
                if (uri.IsFile || uri.IsUnc)
                {
                    return true;
                }
                else if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                {
                    return false;
                }
            }

            throw new Exception("Unable to determine url kind: " + url);
        }

        public static string LocalPath(string urlOrLocalPath)
        {
            try
            {
                var uri = new Uri(urlOrLocalPath);
                return uri.LocalPath;
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to determine local path for [{urlOrLocalPath}]: {e.Message}");
            }
        }
        

        public static string Extension(string urlOrLocalPath)
        {
            return Path.GetExtension(LocalPath(urlOrLocalPath));
        }


        //public static bool LocalFileExists(string urlOrLocalPath) 
        //{
        //    try
        //    {                
        //        return File.Exists(LocalPath(urlOrLocalPath));
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
    }

}
