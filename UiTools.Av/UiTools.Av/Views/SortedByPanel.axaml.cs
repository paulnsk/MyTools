using Avalonia;
using Avalonia.Controls;
using System.Collections.ObjectModel;
using UiTools.Av.ViewModels;

namespace UiTools.Av.Views;

public partial class SortedByPanel : UserControl
{
    public SortedByPanel()
    {
        InitializeComponent();
    }


    public static readonly StyledProperty<ObservableCollection<SortableFieldVm>> SelectedFieldsProperty =
        AvaloniaProperty.Register<SortedByPanel, ObservableCollection<SortableFieldVm>>(
            nameof(SelectedFields));

    public ObservableCollection<SortableFieldVm> SelectedFields
    {
        get => GetValue(SelectedFieldsProperty);
        set => SetValue(SelectedFieldsProperty, value);
    }
}