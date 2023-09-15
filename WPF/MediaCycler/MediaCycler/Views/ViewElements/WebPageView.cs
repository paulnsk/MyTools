using System;
using System.Windows;
using System.Windows.Forms.Integration;
using MediaCycler.ViewModels;

namespace MediaCycler.Views.ViewElements
{
    internal class WebPageView : AMediacontentViewElement
    {

        private readonly WindowsFormsHost _wfh = new WindowsFormsHost();
        private readonly System.Windows.Forms.WebBrowser _wb = new System.Windows.Forms.WebBrowser
        {
            ScriptErrorsSuppressed = true,
            IsWebBrowserContextMenuEnabled = false
        };

        public override FrameworkElement FrameworkElement => _wfh; 
        
        
        public override void Run()
        {
            _wb.Navigate(new Uri(_mediaContent.Location));
        }

        public override void Suspend()
        {
            //web pages cannot be suspended
        }

        public WebPageView(MediaContent mediaContent) : base(mediaContent)
        {
            if (mediaContent.Kind != MediaContentKind.WebPage) throw new Exception($"Imavlid {nameof(MediaContentKind)}:{mediaContent.Kind} for {nameof(WebPageView)}");
            _wfh.Child = _wb;
        }
        

        public event EventHandler PlaybackEnded;



    }
}