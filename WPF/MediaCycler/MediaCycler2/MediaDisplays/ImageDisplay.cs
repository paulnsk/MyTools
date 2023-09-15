using System;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MediaCycler2.Models;

namespace MediaCycler2.MediaDisplays
{
    public class ImageDisplay : IMediaDisplay
    {
        private readonly MediaContent _mediaContent;
        private readonly Image _image = new Image();
        public FrameworkElement FrameworkElement => _image;

        public event EventHandler PlaybackEnded;

        public ImageDisplay(MediaContent mediaContent)
        {
            _mediaContent = mediaContent;
        }

        public void Load()
        {
            _image.BeginInit();
            _image.Source = new BitmapImage(new Uri(_mediaContent.Location));
            _image.EndInit();
        }

        public void Pause()
        {
            //images cannot pause
        }
    }
}
