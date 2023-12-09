using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Security.Policy;
using System.Windows.Threading;
using MediaCycler.Models;
using MediaCycler.Utils;
using PropertyChanged;

namespace MediaCycler.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MediaContent:INotifyPropertyChanged
    {
        
        public MediaContentKind Kind { get; }

        protected MediaContent(MediaContentKind kind)
        {
            Kind = kind;


            //DispatcherTimer dt = new DispatcherTimer()
            //{
            //    Interval = TimeSpan.FromSeconds(1)
            //};
            //dt.Tick += Dt_Tick;
            //dt.Start();
        }

        //private void Dt_Tick(object sender, EventArgs e)
        //{
        //    FallbackText = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        //}

        public MediaContent(MediaContentKind kind, string location) : this(kind)
        {
            Location = location;
            if (location.IsLocalUrl())
            {
                if (!File.Exists(UrlUtils.LocalPath(location)))
                {
                    FallbackText = "File not found: " + location;
                }
            }
        }

        /// <summary>
        /// Url for web sites, local file path for local files and cached media. 
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Applies Stretch = Stretch.Fill (images only)
        /// </summary>
        public bool StretchToFill { get; set; }

        /// <summary>
        /// Optional error message to display when actual content cannot be shown
        /// </summary>
        public string FallbackText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
