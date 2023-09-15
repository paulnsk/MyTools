using System;
using System.Windows;
using System.Windows.Controls;
using MediaCycler2.Models;

namespace MediaCycler2.MediaDisplays
{
    public class VideoDisplay : IMediaDisplay
    {
        private readonly MediaContent _mediaContent;
        private readonly MediaElement _video = new MediaElement();
        public FrameworkElement FrameworkElement => _video;

        public VideoDisplay(MediaContent mediaContent)
        {
            if (mediaContent.Kind != MediaContentKind.Video) throw new Exception($"Imavlid {nameof(MediaContentKind)}:{mediaContent.Kind} for {nameof(VideoDisplay)}");
            _mediaContent = mediaContent;
            _video.MediaEnded += Video_MediaEnded;
        }

        private void Video_MediaEnded(object sender, RoutedEventArgs e)
        {
            PlaybackEnded?.Invoke(this, EventArgs.Empty);
        }

        public void Load()
        {
            _video.Source = new Uri(_mediaContent.Location);
        }

        public void Pause()
        {
            var lb = _video.LoadedBehavior;
            _video.LoadedBehavior = MediaState.Manual;
            _video.Pause();
            _video.LoadedBehavior = lb;
        }

        public event EventHandler PlaybackEnded;
    }
}