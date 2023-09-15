using System;
using System.Windows;
using System.Windows.Forms.Integration;
using MediaCycler2.Models;

namespace MediaCycler2.MediaDisplays
{
    public class WebPageDisplay : IMediaDisplay
    {
        private readonly MediaContent _mediaContent;
        private readonly WindowsFormsHost _wfh;
        private readonly System.Windows.Forms.WebBrowser _wb;

        public FrameworkElement FrameworkElement => _wfh;

        public WebPageDisplay(MediaContent mediaContent)
        {
            if (mediaContent.Kind != MediaContentKind.WebPage) throw new Exception($"Imavlid {nameof(MediaContentKind)}:{mediaContent.Kind} for {nameof(WebPageDisplay)}");
            _mediaContent = mediaContent;
            _wfh = new WindowsFormsHost();
            _wb = new System.Windows.Forms.WebBrowser()
            {
                ScriptErrorsSuppressed = true,
                IsWebBrowserContextMenuEnabled = false
            };
            _wfh.Child = _wb;
        }

        public void Load()
        {
            _wb.Navigate(new Uri(_mediaContent.Location));
        }
        
        public void Pause()
        {
            //web pages cannot pause
        }

        public event EventHandler PlaybackEnded;
    }
}