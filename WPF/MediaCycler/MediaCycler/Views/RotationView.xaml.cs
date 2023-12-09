using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Threading;
using MediaCycler.ViewModels;

namespace MediaCycler.Views
{
    /// <summary>
    /// Interaction logic for RotationView.xaml
    /// </summary>
    public partial class RotationView : UserControl
    {

        private class RotationViewItem
        {
            public MediaContentView MediaContentView { get; set; }
            public double DurationMilliseconds { get; set; }
        }


        private DispatcherTimer Timer { get; set; }
        private List<RotationViewItem> Items { get; set; } = new List<RotationViewItem>();
        private int CurrentItemIndex { get; set; } = -1;
        private DateTime ItemStartedAt { get; set; } = DateTime.MinValue;

        public RotationView()
        {
            InitializeComponent();
            Timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            Timer.Tick += Timer_Tick;
        }
        public void AddItem(MediaContent mediaContent, double durationMilliseconds = 3600000)
        {
            var mcv = new MediaContentView { MediaContent = mediaContent };
            mcv.View.PlaybackEnded += View_PlaybackEnded;
            Items.Add(new RotationViewItem { MediaContentView = mcv, DurationMilliseconds = durationMilliseconds });
        }

        private void View_PlaybackEnded(object sender, EventArgs e)
        {
            //assuming is is the current item that fired the event - this will cause next item to start on next timer tick
            ItemStartedAt = DateTime.MinValue;
        }

        public void Start()
        {
            //forcing next item to start immediately
            // (otherwise, if paused and restarted within the current item's duration time,
            // the control would show empty screen until duration time fully expires)
            ItemStartedAt = DateTime.MinValue;

            if (!Timer.IsEnabled) Timer.Start();
        }

        public void Pause()
        {
            Timer.Stop();
            Tv.CurrentFrame = new Grid();
        }

        private bool NextIsReady()
        {
            if (Items.Count <= 0) return false;
            var currentItem = CurrentItem();
            if (currentItem == null) return true;
            return (DateTime.Now - ItemStartedAt).TotalMilliseconds > currentItem.DurationMilliseconds;
        }

        private void Next()
        {
            if (NextIsReady())
            {
                CurrentItemIndex++;
                if (CurrentItemIndex >= Items.Count) CurrentItemIndex = 0;
                ItemStartedAt = DateTime.Now;
                Tv.CurrentFrame = CurrentItem().MediaContentView;
            }
        }

        private RotationViewItem CurrentItem() => CurrentItemIndex > -1 ? Items[CurrentItemIndex] : null;

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (NextIsReady()) Next();
        }





    }
}
