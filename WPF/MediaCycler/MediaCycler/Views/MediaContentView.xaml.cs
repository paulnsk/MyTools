using System.Windows;
using System.Windows.Controls;
using MediaCycler.ViewModels;
using MediaCycler.Views.ViewElements;

namespace MediaCycler.Views
{
    /// <summary>
    /// Interaction logic for MediaContentView.xaml
    /// </summary>
    public partial class MediaContentView : UserControl
    {
        private MediaContent _mediaContent;

        public MediaContent MediaContent
        {
            get => _mediaContent;
            set
            {
                _mediaContent = value;
                View = MediaContentViewElementFactory.Create(value);
                ContentGrid.Children.Clear();
                ContentGrid.Children.Add(View.FrameworkElement);
                DataContext = value;
            }
        }

        public MediaContentView()
        {
            InitializeComponent();
        }



        public IMediacontentViewElement View { get; set; }

        
    }
}
