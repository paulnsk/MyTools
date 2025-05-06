using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using UiTools.Av.ViewModels;

namespace UiTools.Av.Views;

public partial class SortbyDropdown : UserControl
{
    public SortbyDropdown()
    {
        InitializeComponent();
    }


    public static readonly StyledProperty<ObservableCollection<SortableFieldVm>> SortableFieldsProperty =
        AvaloniaProperty.Register<SortbyDropdown, ObservableCollection<SortableFieldVm>>(nameof(SortableFields));

    public ObservableCollection<SortableFieldVm> SortableFields
    {
        get => GetValue(SortableFieldsProperty);
        set => SetValue(SortableFieldsProperty, value);
    }

    private void BtnCloseFlyout_Click(object sender, RoutedEventArgs e)
    {
        // Закрытие Flyout
        if (SortByButton.Flyout is Flyout flyout)
        {
            flyout.Hide();
        }
    }

}