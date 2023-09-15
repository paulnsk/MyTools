using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UiTools.Uno.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UiTools.Uno.Views
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
