using MediaCycler2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaCycler2.Controls
{


    /// <summary>
    /// Interaction logic for RotationControl.xaml
    /// </summary>
    public partial class RotationControl : UserControl
    {


        class RotationControlItem
        {
            public RotatableMedia Media { get; set; }
            public MediaControl MediaControl { get; set; }
        }


        public RotationControl()
        {
            InitializeComponent();
        }

        List<RotationControlItem> _items = new List<RotationControlItem>();
        private bool _paused;

        public void AddItem(RotatableMedia media)
        {
            var mediaControl = new MediaControl(media.Content);


            if (media.Duration.HasValue)
            {
                media.DurationExpired += Item_DurationExpiredOrPlaybackEnded;
            }
            else
            {
                if (media.Content.Kind == MediaContentKind.Video)
                {
                    mediaControl.PlaybackEnded += Item_DurationExpiredOrPlaybackEnded;
                }
                else throw new Exception($"Must specify {nameof(RotatableMedia.Duration)} except for videos");
            }
            
            _items.Add(new RotationControlItem { Media=media, MediaControl=mediaControl});
        }

        public void Start()
        {
            if (!_paused)
            {
                //starting first time
                if (_items.Any(x => x.Media.IsRunning)) throw new Exception($"{nameof(RotationControl)} is already running");
                if (_items.Count == 0) throw new Exception($"{nameof(RotationControl)} is empty");
                _items[0].Media.Start();
                Tv.CurrentFrame = _items[0].MediaControl;
            }
            else
            {
                _items[_currentIndex].Media.Start();
                Tv.CurrentFrame = _items[_currentIndex].MediaControl;
                _paused = false;
            }
        }

        int _currentIndex = 0;
        private void Item_DurationExpiredOrPlaybackEnded(object sender, EventArgs e)
        {
            RotationControlItem item;
            if (sender is RotatableMedia rm) item = _items.First(x => x.Media == rm);
            else if (sender is MediaControl mc) item = _items.First(x => x.MediaControl == mc);
            else throw new Exception($"Something went wrong: item is not supposed to be {sender.GetType()}");

            _currentIndex = _items.IndexOf(item);
            _currentIndex++;
            if (_currentIndex >= _items.Count) _currentIndex = 0;
         
            if (!_paused)
            {
                _items[_currentIndex].Media.Start();
                Tv.CurrentFrame = _items[_currentIndex].MediaControl;
            }
        }

        public void Pause()
        {
            if (_items.Count == 0) return;
            if (_paused) return;
            _paused = true;
            Tv.CurrentFrame = new Grid();
            _items[_currentIndex].Media.Stop();
            _items[_currentIndex].MediaControl.Stop();
        }

    }
}
