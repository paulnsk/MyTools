using PropertyChanged;
using System.ComponentModel;


namespace MediaCycler2.Models
{
    [AddINotifyPropertyChangedInterface]
    public class MediaContent:INotifyPropertyChanged
    {
        
        public MediaContentKind Kind { get; }

        protected MediaContent(MediaContentKind kind)
        {
            Kind = kind;            
        }

        public MediaContent(MediaContentKind kind, string location) : this(kind)
        {
            Location = location;
        }

        /// <summary>
        /// Url for web sites, local file path for local files and cached media. 
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Optional error message to display when actual content cannot be shown
        /// </summary>
        public string FallbackText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
