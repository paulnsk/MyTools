using System;
using System.Windows;

namespace MediaCycler2.MediaDisplays
{
    public interface IMediaDisplay
    {
        FrameworkElement FrameworkElement { get; }

        void Load();

        void Pause();

        /// <summary>
        /// this may or may not fire depending on content
        /// </summary>
        event EventHandler PlaybackEnded;

    }

}
