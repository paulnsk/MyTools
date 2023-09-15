using PropertyChanged;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using MediaCycler.Services;
using MediaCycler.ViewModels;


namespace MediaCyclerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        MediaCache _cache = new MediaCache();

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {            
            InitializeComponent();
            _cache.ClearTempFiles();

            DataContext = this;

            DispatcherTimer dt=new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(1000) };
            dt.Tick += Dt_Tick;
            dt.Start();

        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            Tekst = DateTime.Now.ToString();            
        }

        private string PrintObject(object o)
        {            
            return string.Join("\n", o.GetType().GetProperties().Select(p => $"[{p.Name}]: {p.GetValue(o)}"));
        }

        private void Log(string s)
        {
            
            Tb.AppendText(s + "\n");
            Tb.ScrollToEnd();
        }

        public string Tekst { get; set; }


        private void BtnGetFileFromCache_Click(object sender, RoutedEventArgs e)
        {
            var result = _cache.GetCachedMedia("https://intouchposcontentserver.s3.us-west-2.amazonaws.com/Milano/DualScreen/MEN17102_DisplayGraphics.mp4");
            Log(PrintObject(result));
            Log("");            
        }

        private void BtnPopulaterotationcontrol_Click(object sender, RoutedEventArgs e)
        {
            Rv.AddItem(new MediaContent(MediaContentKind.Image, @"k:\__FOTKI__\05\2005.05.11\P5113501.JPG"), 3000);
            Rv.AddItem(new MediaContent(MediaContentKind.Image, @"k:\__FOTKI__\05\2005.05.11\P5113499.JPG"), 3000);
            Rv.AddItem(new MediaContent(MediaContentKind.Video, @"k:\Лазер\a\av-121-15.avi"), 3000);
            Rv.AddItem(new CachedMediaContent(MediaContentKind.Video, _cache, "https://intouchposcontentserver.s3.us-west-2.amazonaws.com/Milano/DualScreen/MEN17102_DisplayGraphics.mp4"));
            Rv.AddItem(new MediaContent(MediaContentKind.Image, @"k:\__FOTKI__\05\2005.05.11\P5093493.JPG"), 3000);
            Rv.AddItem(new CachedMediaContent(MediaContentKind.Image, _cache, "https://intouchposcontentserver.s3.us-west-2.amazonaws.com/Milano/DualScreen/MEN17102_DisplayGraphics_09.jpg"), 4000);
            Rv.AddItem(new MediaContent(MediaContentKind.WebPage, "https://google.com/"), 4000);
            Rv.Start();

            //Rc.AddItem(new RotatableMedia { Duration = 3000, Content = new MediaContent { Kind = MediaContentKind.Image, LocalFilePath = @"k:\__FOTKI__\05\2005.05.11\P5113501.JPG" } });
            //Rc.AddItem(new RotatableMedia { Duration = 3000, Content = new MediaContent { Kind = MediaContentKind.Image, LocalFilePath = @"k:\__FOTKI__\05\2005.05.11\P5113499.JPG" } });
            //Rc.AddItem(new RotatableMedia { Duration = 3000, Content = new MediaContent { Kind = MediaContentKind.Image, LocalFilePath = @"k:\__FOTKI__\05\2005.05.11\P5093493.JPG" } });

            //Rc.AddItem
            //    (
            //        new RotatableMedia
            //        {
            //            Duration = 20000,
            //            Content = new MediaContent
            //            {
            //                Kind = MediaContentKind.Video,
            //                Location = "k:\\androed\\KZ_droed\\06\\20220512_183104.mp4"
            //            }
            //        }
            //    );


            //Rc.AddItem
            //    (
            //        new RotatableMedia
            //        {
            //            Duration = null,
            //            Content = new CachedMediaContent(MediaContentKind.Video, _cache, "https://intouchposcontentserver.s3.us-west-2.amazonaws.com/Milano/DualScreen/MEN17102_DisplayGraphics.mp4")

            //        }
            //    );
            //Rc.AddItem
            //    (
            //        new RotatableMedia
            //        {
            //            Duration = 3000,
            //            Content = new CachedMediaContent(MediaContentKind.Image, _cache, "https://intouchposcontentserver.s3.us-west-2.amazonaws.com/Milano/DualScreen/MEN17102_DisplayGraphics_07.jpg")                        
            //        }
            //    );
            //Rc.AddItem
            //    (
            //        new RotatableMedia
            //        {
            //            Duration = 3000,
            //            Content = new CachedMediaContent(MediaContentKind.Image, _cache, "https://intouchposcontentserver.s3.us-west-2.amazonaws.com/Milano/DualScreen/MEN17102_DisplayGraphics_09.jpg")                        
            //        }
            //    );
            //Rc.AddItem
            //    (
            //        new RotatableMedia
            //        {
            //            Duration = 3000,
            //            Content = new MediaContent(MediaContentKind.WebPage, "https://google.com")
            //        }
            //    );
            ////Rc.AddItem(new RotatableMedia { Duration = 3000, Content = new MediaContent { Kind = MediaContentKind.Image, LocalFilePath = @"k:\__FOTKI__\05\2005.05.11\P5113499.JPG" } });
            ////Rc.AddItem(new RotatableMedia { Duration = 3000, Content = new MediaContent { Kind = MediaContentKind.Image, LocalFilePath = @"k:\__FOTKI__\05\2005.05.11\P5093493.JPG" } });

            //Rc.Start();
        }

        private void BtnSuspend_OnClick(object sender, RoutedEventArgs e)
        {
            //mcv.View.Suspend();
        }

        private void BtnRun_OnClick(object sender, RoutedEventArgs e)
        {
            //mcv.View.Run();
        }

        private void BtnLoadMedia_OnClick(object sender, RoutedEventArgs e)
        {
            //mcv.MediaContent = new CachedMediaContent(MediaContentKind.Video, _cache,
            //    "https://intouchposcontentserver.s3.us-west-2.amazonaws.com/Milano/DualScreen/MEN17102_DisplayGraphics.mp4");
            //mcv.MediaContent = new MediaContent(MediaContentKind.Image, @"k:\ОшибкаОплаты.jpg");
        }
    }
}
