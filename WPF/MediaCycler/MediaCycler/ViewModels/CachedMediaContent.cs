using System;
using System.Windows.Threading;
using MediaCycler.Models;
using MediaCycler.Services;
using MediaCycler.Utils;

namespace MediaCycler.ViewModels
{
    public class CachedMediaContent : MediaContent
    {
        private string Url { get; set; }

        private readonly MediaCache _cache;
        private MediaCacheResult _mediaCacheResult;
        private readonly DispatcherTimer _timer;

        private void AttemptGetFromCache()
        {            
            _mediaCacheResult = _cache.GetCachedMedia(Url);
            if (!_mediaCacheResult.LocalFilePath.IsBlank())
            {
                Location = _mediaCacheResult.LocalFilePath;
                FallbackText = null;
                _timer.Stop();
            }
            else
            {
                Location = null;
                FallbackText = _mediaCacheResult.ErrorMessage;
                _timer.Start();
            }
        }


        public CachedMediaContent(MediaContentKind kind, MediaCache cache, string url) : base(kind)
        {
            Url = url;
            _cache = cache;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _timer.Tick += Timer_Tick;

            AttemptGetFromCache();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            AttemptGetFromCache();
        }
    }

}
