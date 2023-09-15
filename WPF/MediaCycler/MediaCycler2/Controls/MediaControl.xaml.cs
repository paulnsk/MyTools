using MediaCycler2.Models;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MediaCycler2.Controls
{



    /// <summary>
    /// Interaction logic for MediaControl.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MediaControl : UserControl, INotifyPropertyChanged
    {
        public MediaControl()
        {
            //empty ctor for designtime datacontext
            DataContext = this;
        }

        public MediaControl(MediaContent mediaContent) : this()
        {
            InitializeComponent();
            MediaContent = mediaContent;
            MediaContent.PropertyChanged += MediaContent_PropertyChanged;
            CreateViewerByKind();
            SetProperties();
            
            //todo доразобраться с байдингами и вычистить
            var dt = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
            dt.Tick += Dt_Tick;
            dt.Start();
        }

        int i = 0;
        private void Dt_Tick(object sender, EventArgs e)
        {
            i++;
            //ProstoTekst = MediaContent.FallbackText;
        }

        public AMediaContentControl Viewer { get; set; }

        private void CreateViewerByKind()
        {
            switch (MediaContent.Kind)
            {
                case MediaContentKind.Image:
                    Viewer = new ImageControl();                    
                    ContentGrid.Children.Add(Viewer);
                    break;
                case MediaContentKind.Video:
                    Viewer = new VideoControl();
                    ((VideoControl)Viewer).VideoEnded += (s, e) => { PlaybackEnded?.Invoke(this, EventArgs.Empty); };
                    ContentGrid.Children.Add(Viewer);
                    break;
                case MediaContentKind.WebPage:
                    Viewer = new WebControl();                    
                    ContentGrid.Children.Add(Viewer);
                    break;
            }
        }


        private void SetProperties()
        {
            FallBackText = MediaContent.FallbackText;
            var localFileExists = !string.IsNullOrEmpty(MediaContent.Location);
            FallbackVisible = !localFileExists;
            if (localFileExists) Viewer.Load(MediaContent.Location);         
        }

        private void MediaContent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //todo какого хера пропертицхангед не прокидывается в юай??
            //fuck knows why but direct binding to MediaContent.Whatever is not working
            SetProperties();
        }

        internal void Stop()
        {
            if (MediaContent.Kind == MediaContentKind.Video)
            {
                ((VideoControl)Viewer).Stop();
            }
        }

        public MediaContent MediaContent { get; }

        public bool FallbackVisible { get; set; }        

        public string FallBackText { get; set; }

        public event EventHandler PlaybackEnded;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
