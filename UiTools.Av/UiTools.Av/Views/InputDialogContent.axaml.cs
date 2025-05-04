using Avalonia;
using Avalonia.Controls;

namespace UiTools.Av.Views;

public partial class InputDialogContent : UserControl
{
    public InputDialogContent()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<string> PromptProperty =
        AvaloniaProperty.Register<InputDialogContent, string>(nameof(Prompt));

    public string Prompt
    {
        get => GetValue(PromptProperty);
        set => SetValue(PromptProperty, value);
    }

    public static readonly StyledProperty<string> InputProperty =
        AvaloniaProperty.Register<InputDialogContent, string>(nameof(Input));

    public string Input
    {
        get => GetValue(InputProperty);
        set => SetValue(InputProperty, value);
    }
}
