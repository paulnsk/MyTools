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

    public static readonly StyledProperty<ObservableCollection<StackableMessage>> Items22SourceProperty =
        AvaloniaProperty.Register<StackableMessageView, ObservableCollection<StackableMessage>>(nameof(Items22Source));

    public ObservableCollection<StackableMessage> Items22Source
    {
        get => GetValue(Items22SourceProperty);
        set => SetValue(Items22SourceProperty, value);
    }

    
}