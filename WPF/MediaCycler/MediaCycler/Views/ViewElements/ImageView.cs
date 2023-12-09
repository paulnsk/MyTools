using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MediaCycler.ViewModels;

namespace MediaCycler.Views.ViewElements
{
    internal class ImageView : AMediacontentViewElement
    {

        private readonly Image _image = new Image();
        public override FrameworkElement FrameworkElement => _image;

        public ImageView(MediaContent mediaContent) : base(mediaContent)
        {
            if (mediaContent.Kind != MediaContentKind.Image) throw new Exception($"Imavlid {nameof(MediaContentKind)}:{mediaContent.Kind} for {nameof(ImageView)}");
        }

        public override void Run()
        {
            _image.BeginInit();
            _image.Source = new BitmapImage(new Uri(_mediaContent.Location));
            if (_mediaContent.StretchToFill) _image.Stretch = Stretch.Fill;
            _image.EndInit();
        }

        public override void Suspend()
        {
            //images cannot be suspended
        }

    }
}
