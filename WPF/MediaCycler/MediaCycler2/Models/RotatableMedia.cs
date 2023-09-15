using System;
using System.Windows.Threading;

namespace MediaCycler2.Models
{
    public class RotatableMedia
    {
               

        public MediaContent Content { get; set; }
        public double? Duration { get; set; }

        public bool IsRunning { get; private set; }


        public void Start()
        {
            
            IsRunning = true;
            
            if (Duration.HasValue) 
            { 
                DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(Duration.Value) };
                timer.Tick += (s, e) => 
                {
                    if (IsRunning) DurationExpired?.Invoke(this, EventArgs.Empty);
                    IsRunning = false;
                    ((DispatcherTimer)s).Stop();
                };

                timer.Start();
            }
        }

        /// <summary>
        /// prevents DurationExpired event from firing
        /// </summary>
        internal void Stop()
        {
            IsRunning = false;
        }

        public event EventHandler DurationExpired;

    }
}
