using Avalonia.Controls;
using System.Collections.ObjectModel;
using Avalonia;
using UiTools.Av.ViewModels;

namespace UiTools.Av.Views;

public partial class StackableMessageView : UserControl
{
    public StackableMessageView()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<ObservableCollection<StackableMessage>> ItemsSourceProperty =
        AvaloniaProperty.Register<StackableMessageView, ObservableCollection<StackableMessage>>(nameof(ItemsSource));

    public ObservableCollection<StackableMessage> ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    
    
}