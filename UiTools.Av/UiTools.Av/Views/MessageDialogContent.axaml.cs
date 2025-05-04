using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using System.Globalization;
using System;
using System.IO;
using System.Linq;
using Avalonia.Controls.Presenters;
using Avalonia.Input.Platform;
using Avalonia.VisualTree;
using FluentAvalonia.UI.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;

namespace UiTools.Av.Views;


public partial class MessageDialogContent : UserControl
{
    public MessageDialogContent()
    {
        InitializeComponent();
        UpdateIcon(); // начальная инициализация
    }

    public static readonly StyledProperty<string> ErrorMessageProperty =
        AvaloniaProperty.Register<MessageDialogContent, string>(nameof(Message));

    public string Message
    {
        get => GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
    }

    public static readonly StyledProperty<string> IconTextProperty =
        AvaloniaProperty.Register<MessageDialogContent, string>(nameof(IconText));

    public string IconText
    {
        get => GetValue(IconTextProperty);
        private set => SetValue(IconTextProperty, value);
    }

    public static readonly StyledProperty<IBrush> IconColorProperty =
        AvaloniaProperty.Register<MessageDialogContent, IBrush>(nameof(IconColor));

    public IBrush IconColor
    {
        get => GetValue(IconColorProperty);
        private set => SetValue(IconColorProperty, value);
    }

    public static readonly StyledProperty<MessageDialogIconKind> IconKindProperty =
        AvaloniaProperty.Register<MessageDialogContent, MessageDialogIconKind>(nameof(IconKind));

    public MessageDialogIconKind IconKind
    {
        get => GetValue(IconKindProperty);
        set
        {
            SetValue(IconKindProperty, value);
            UpdateIcon();
        }
    }

    private void UpdateIcon()
    {
        switch (IconKind)
        {
            case MessageDialogIconKind.Info:
                IconText = "ℹ️";
                IconColor = Brushes.DodgerBlue;
                break;
            case MessageDialogIconKind.Error:
                IconText = "❌";
                IconColor = Brushes.Red;
                break;
            case MessageDialogIconKind.Warning:
                IconText = "⚠️";
                IconColor = Brushes.Goldenrod;
                break;
            case MessageDialogIconKind.Question:
                IconText = "❓"; //всегда красный
                //IconText= "?";  //красится как положено в даркцыан
                IconColor = Brushes.DarkCyan;
                break;
            default:
                IconText = "?";
                IconColor = Brushes.Gray;
                break;
        }
    }
}


public enum MessageDialogIconKind
{
    Info,
    Error,
    Warning,
    Question
}


public class HalfConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double d and > 0)
            return d * 0.8; 
        throw new Exception($"Unable to get value for {nameof(HalfConverter)}");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
