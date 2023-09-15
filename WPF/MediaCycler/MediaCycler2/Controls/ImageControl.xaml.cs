using System;
using System.Windows.Media.Imaging;

namespace MediaCycler2.Controls
{

    /// <summary>
    /// Interaction logic for ImageControl.xaml
    /// </summary>
    public partial class ImageControl : AMediaContentControl
    {
        public ImageControl()
        {
            InitializeComponent();
        }

        public override void Load(string path)
        {
            I.BeginInit();
            I.Source = new BitmapImage(new Uri(path));
            I.EndInit();
        }
    }
}
