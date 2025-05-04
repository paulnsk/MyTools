using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UiTools.Uno.My.ViewModels;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UiTools.Uno.My.Views
{
    public sealed partial class SortedByPanel : UserControl
    {
        public SortedByPanel()
        {
            this.InitializeComponent();
        }


        public ObservableCollection<SortableFieldVm> SelectedFields
        {
            get { return (ObservableCollection<SortableFieldVm>)GetValue(SelectedFieldsProperty); }
            set { SetValue(SelectedFieldsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedFieldsProperty =
            DependencyProperty.Register(nameof(SelectedFields), typeof(ObservableCollection<SortableFieldVm>), typeof(SortedByPanel), new PropertyMetadata(new ObservableCollection<SortableFieldVm>()));
        
    }
}
