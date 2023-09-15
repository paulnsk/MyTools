using System;
using System.Windows;

namespace MediaCycler.Views.ViewElements
{
    public interface IMediacontentViewElement
    {
        FrameworkElement FrameworkElement { get; }

        
        void Run();

        void Suspend();
        
        event EventHandler PlaybackEnded;
    }
}
