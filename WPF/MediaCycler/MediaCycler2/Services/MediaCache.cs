using MediaCycler2.Models;
using MediaCycler2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace MediaCycler2.Services
{

    //todo removing file from cache does not cause redownload!

    public class MediaCache
    {

        private const string tempExt = ".temp";

        private class MediaCacheItemState
        {
            public bool? Success { get; set; }
            public string DownloadError { get; set; }
        }

        public string Dir { get; set; } = Path.Combine(PathUtils.ExeDir(), "MediaCache");

        
        //empty string means download in progress
        private Dictionary<string, MediaCacheItemState> _itemStates = new Dictionary<string, MediaCacheItemState>();


        public string LocalFilePath(string url)
        {

            return Path.Combine(Dir, CryptoUtils.Md5String(url) + UrlUtils.Extension(url));
        }

        public string LocalFileTempPath(string url)
        {
            return $"{LocalFilePath(url)}.{DateTime.Now:yyyy_MM_dd_HH_mm_ss_fff}{tempExt}";
        }

        /// <summary>
        /// Returns cached local file for specified url. Downloads item if not exists.
        /// When called for the first time, causes the item to be re-downloaded even if it is already in cache.
        /// (existing file is served during the download)
        /// If a download fails then it is never retried until the app is restarted.
        /// URLs can be either local or remote. Local files are served directly from their location without copying them into cache
        /// </summary>        
        /// <param name="url">
        /// http://host.domain/path/filename.ext 
        /// or /// file:///d:\\dir\\filename.ext 
        /// or simply /// d:\\dir\\filename.ext
        /// </param>
        /// <returns></returns>
        public MediaCacheResult GetCachedMedia(string url)
        {
            var result = new MediaCacheResult();

            if (url.IsLocalUrl())
            {
                var localFilePath = UrlUtils.LocalPath(url);
                result.IsInitializing = false;
                if (File.Exists(localFilePath)) result.LocalFilePath = localFilePath;
                else result.ErrorMessage = "File not found: " + url;
                return result;
            }


            var localCachedFilePath = LocalFilePath(url);

            if (File.Exists(localCachedFilePath))
            {                
                result.LocalFilePath = localCachedFilePath;
                //re-downloading file to get a fresh version every time the app starts and calls the cache for the 1st time
                //If redownload fails we will keep serving cached file
                if (!_itemStates.ContainsKey(url)) BeginDownload(url);
            }
            else
            {
                if (_itemStates.TryGetValue(url, out var item))
                {
                    if (item.Success == false)
                    {
                        result.ErrorMessage = item.DownloadError;
                    }
                    else if (item.Success == true)
                    {
                        result.IsInitializing = true;
                        result.ErrorMessage = $"File has been deleted from cachem starting re-dowload from [{url}]...";
                        BeginDownload(url);
                    }
                    else
                    {
                        result.IsInitializing = true;
                        result.ErrorMessage = $"[{url}] is still being downloaded...";
                    }
                }
                else
                {
                    result.IsInitializing = true;
                    result.ErrorMessage = $"Starting dowload of [{url}]...";
                    BeginDownload(url);
                }
            }

            return result;
        }

        public void ClearTempFiles()
        {
            if (!Directory.Exists(Dir)) return;
            if (_itemStates.Any(x => !x.Value.Success != false)) throw new Exception("Cannot call " + nameof(ClearTempFiles) + " while files are being downloaded");
            var toRemove = Directory.GetFiles(Dir, $"*{tempExt}").ToList();
            foreach ( var filePath in toRemove) File.Delete(filePath); 
        }


        private void BeginDownload(string url)
        {
            //fire and forget
            _ = Task.Run
            (
                async () =>
                {
                    try
                    {
                        _itemStates[url] = new MediaCacheItemState();

                        var tempPath = LocalFileTempPath(url);
                        if(!Directory.Exists(Dir)) Directory.CreateDirectory(Dir);
                        if (!Directory.Exists(Dir)) throw new Exception($"Unable to find or create directory {Dir}");
                        await HttpEx.DownloadFile(url, tempPath);
                        TryReplaceFileWithNew(LocalFilePath(url), tempPath);
                        
                        _itemStates[url].Success = true;
                    }
                    catch (Exception e)
                    {
                        _itemStates[url].Success = false;
                        _itemStates[url].DownloadError = "Download error: " + e.Message;
                    }
                }
            );            

        }

        /// <summary>
        /// if old file is being used then this will keep trying until it is released
        /// </summary>
        /// <param name="oldFilePath"></param>
        /// <param name="newFilePath"></param>
        private void TryReplaceFileWithNew(string oldFilePath, string newFilePath) 
        { 
            bool DoTry()
            {
                if (File.Exists(oldFilePath)) try { File.Delete(oldFilePath); } catch { }
                if (!File.Exists(oldFilePath) && File.Exists(newFilePath))
                {
                    File.Move(newFilePath, oldFilePath);
                    return true;
                }
                return false;
            }

            if (!DoTry())
            {
                var timer = new Timer(500);
                timer.Elapsed += (ъ, ы) =>
                {
                    if (DoTry())
                    {
                        ((Timer)ъ).Stop();
                    }
                };
                timer.Start();
            }
        }

    }
}
