using System;
using System.ComponentModel;
using System.Timers;

namespace MyTools
{
    /// <summary>
    /// Unlike an ordinary System.Timers.Timer this wrapper class provides support for long actions (such as connecting to a remote DB) by pausing itself while the long
    /// procedure is being executed. 
    /// </summary>
    public class LameTimer
    {
        public readonly Timer InternalTimer;
        private bool _enabled;
        private DateTime _lastTickFinishedAt;

        /// <summary>
        /// Pass an <paramref name="uiSyncObject"/> such as a Form to cause timer events to fire on the main UI thread.
        /// (By default they will fire on threadpool.)
        /// Set TickFunc to an Action that will execute on internalTimer.Elapsed.
        /// Call Start() or set Enabled=true to start the timer
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="uiSyncObject"></param>
        public LameTimer(double interval, ISynchronizeInvoke uiSyncObject = null)
        {
            InternalTimer = new Timer { Interval = interval };
            InternalTimer.Elapsed += InternalTimer_Elapsed;
            if (uiSyncObject != null) InternalTimer.SynchronizingObject = uiSyncObject;
        }


        public event EventHandler<LameIntervalEventArgs> OnMeasureActualInterval;

        private void InternalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            InternalTimer.Stop();

            if (!Enabled) return;

            TickFunc?.Invoke();

            var now = DateTime.Now;
            
            OnMeasureActualInterval?.Invoke(this, new LameIntervalEventArgs { Interval = now - _lastTickFinishedAt });
            
            _lastTickFinishedAt = now;


            InternalTimer.Start();
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    _lastTickFinishedAt = DateTime.Now;
                    InternalTimer.Start();
                }
                else InternalTimer.Stop();
            }
        }


        public void Stop()
        {
            Enabled = false;
        }

        public void Start()
        {
            Enabled = true;
        }

        public Action TickFunc { get; set; }
        
    }

    public class LameIntervalEventArgs : EventArgs
    {
        public TimeSpan Interval { get; set; }
    }


}