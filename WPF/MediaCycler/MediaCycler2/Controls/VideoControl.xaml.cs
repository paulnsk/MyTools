using System;
using System.Windows;
using System.Windows.Controls;

namespace MediaCycler2.Controls
{
    /// <summary>
    /// Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class VideoControl : AMediaContentControl
    {
        public VideoControl()
        {
            InitializeComponent();
        }

        public override void Load(string path)
        {            
            M.Source = new Uri(path);
        }

        private void M_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            VideoEnded?.Invoke(this, EventArgs.Empty);            
        }

        public event EventHandler VideoEnded;

        public void Stop()
        {
            var lb = M.LoadedBehavior;
            M.LoadedBehavior = MediaState.Manual;
            M.Stop();
            M.LoadedBehavior = lb;
        }
    }
}
