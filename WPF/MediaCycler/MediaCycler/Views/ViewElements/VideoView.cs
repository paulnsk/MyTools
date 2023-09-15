using System;
using System.Windows;
using System.Windows.Controls;
using MediaCycler.ViewModels;

namespace MediaCycler.Views.ViewElements
{
    internal class VideoView : AMediacontentViewElement
    {
        
        private readonly MediaElement _video = new MediaElement();
        public override FrameworkElement FrameworkElement => _video;

        public override void Run()
        {
            _video.LoadedBehavior = MediaState.Manual;
            _video.Source = new Uri(_mediaContent.Location);
            _video.Play();
        }

        public override void Suspend()
        {
            _video.Stop();
        }


        public VideoView(MediaContent mediaContent) : base(mediaContent)
        {
            if (mediaContent.Kind != MediaContentKind.Video) throw new Exception($"Imavlid {nameof(MediaContentKind)}:{mediaContent.Kind} for {nameof(VideoView)}");
            _video.MediaEnded += Video_MediaEnded;
        }
        

        private void Video_MediaEnded(object sender, RoutedEventArgs e)
        {
            OnPlayBackEnded();
        }

    }
}