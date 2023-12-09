using System;
using System.Windows;
using MediaCycler.Utils;
using MediaCycler.ViewModels;

namespace MediaCycler.Views.ViewElements
{
    internal abstract class AMediacontentViewElement : IMediacontentViewElement
    {
        protected readonly MediaContent _mediaContent;
        public abstract FrameworkElement FrameworkElement { get; }
        public abstract void Run();

        protected AMediacontentViewElement(MediaContent mediaContent)
        {
            _mediaContent = mediaContent;
            _mediaContent.PropertyChanged += MediaContent_PropertyChanged;
            MediaContent_PropertyChanged(null, null);
        }

        private void MediaContent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //don't care which property changed, attempt to show the content
            if (_mediaContent.FallbackText.IsBlank() && !_mediaContent.Location.IsBlank()) Run();
        }

        public abstract void Suspend();

        public event EventHandler PlaybackEnded;

        public void OnPlayBackEnded()
        {
            PlaybackEnded?.Invoke(this, EventArgs.Empty);
        }

    }
}