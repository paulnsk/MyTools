﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using UiTools.Uno.ViewModels;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UiTools.Uno.Views
{
    public sealed partial class SortbyDropdown : UserControl
    {
        public SortbyDropdown()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<SortableFieldViewModel> SortableFields
        {
            get { return (ObservableCollection<SortableFieldViewModel>)GetValue(SortableFieldsProperty); }
            set { SetValue(SortableFieldsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SortableFieldsProperty =
            DependencyProperty.Register("SortableFields", typeof(ObservableCollection<SortableFieldViewModel>), typeof(SortbyDropdown), new PropertyMetadata(new ObservableCollection<SortableFieldViewModel>()));


    }
}
