using MediaCycler2.Services;
using System;
using System.Timers;
using System.Windows.Threading;

namespace MediaCycler2.Models
{
    public class CachedMediaContent : MediaContent
    {
        public string Url { get; set; }        

        private readonly MediaCache _cache;
        private MediaCacheResult _mediaCacheResult;
        DispatcherTimer _timer;

        private void SetFields()
        {            
            _mediaCacheResult = _cache.GetCachedMedia(Url);
            if (!string.IsNullOrWhiteSpace(_mediaCacheResult.LocalFilePath))
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
            _timer.Tick += _timer_Tick;

            SetFields();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            SetFields();
        }
    }

}
