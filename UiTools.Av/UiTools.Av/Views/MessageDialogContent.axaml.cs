using Avalonia;
using Avalonia.Controls;
using System;
using Avalonia.Media;

namespace UiTools.Av.Views;


//todo on android text won't fit horizontally
public partial class MessageDialogContent : UserControl
{
    public MessageDialogContent()
    {
        InitializeComponent();


        Loaded += (s, e) =>
        {
            var maxWidthFromRes = Application.Current.FindResource("ContentDialogMaxWidth") as double? ?? 0;
            var maxWidthFromBounds = Bounds.Width;
            var toSet = Math.Min(maxWidthFromRes, maxWidthFromBounds);
            
            var textBlock = this.FindControl<TextBlock>("MessageTextBlock");
            if (textBlock != null)
            {
                textBlock.MaxWidth = (toSet - 50) * 0.9; //50 px for icon
            }
        };



        UpdateIcon(); 
    }

    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<MessageDialogContent, string>(nameof(Message));

    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly StyledProperty<Geometry> IconDataProperty =
        AvaloniaProperty.Register<MessageDialogContent, Geometry>(nameof(IconData));

    public Geometry IconData
    {
        get => GetValue(IconDataProperty);
        private set => SetValue(IconDataProperty, value);
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
            //case MessageDialogIconKind.Info:
            //    IconData = Geometry.Parse("M12,2A10,10 0 1,0 22,12A10,10 0 0,0 12,2M11,17H13V11H11V17M11,9H13V7H11V9Z"); // Info
            //    IconColor = Brushes.DodgerBlue;
            //    break;
            case MessageDialogIconKind.Info:
                IconData = Geometry.Parse("M24,3C12.42,3 3,12.42 3,24C3,35.58 12.42,45 24,45C35.58,45 45,35.58 45,24C45,12.42 35.58,3 24,3Z M22,22H26V33H22V22Z M24,19C22.62,19 21.5,17.88 21.5,16.5C21.5,15.12 22.62,14 24,14C25.38,14 26.5,15.12 26.5,16.5C26.5,17.88 25.38,19 24,19Z");
                IconColor = Brushes.DodgerBlue;
                break;

            case MessageDialogIconKind.Error:
                IconData = Geometry.Parse("M12,2A10,10 0 1,0 22,12A10,10 0 0,0 12,2M13.41,12L17.71,7.71L16.29,6.29L12,10.59L7.71,6.29L6.29,7.71L10.59,12L6.29,16.29L7.71,17.71L12,13.41L16.29,17.71L17.71,16.29L13.41,12Z"); // Error (X)
                IconColor = Brushes.Red;
                break;
            //case MessageDialogIconKind.Warning:
            //    IconData = Geometry.Parse("M1,21H23L12,2"); // Warning (Triangle)
            //    IconColor = Brushes.Goldenrod;
            //    break;
            case MessageDialogIconKind.Warning:
                IconData = Geometry.Parse("M8.982,1.566C8.694,1.081 8.128,0.933 7.678,1.081C7.389,1.164 7.144,1.36 7.018,1.566L0.165,13.233C-0.292,14.011 0.256,15 1.145,15H14.858C15.747,15 16.296,14.011 15.838,13.233L8.982,1.566ZM8,5C7.499,5 7.09,5.428 7.1,5.928L7.45,9.435C7.45,9.715 7.682,9.95 8,9.95C8.318,9.95 8.55,9.715 8.55,9.435L8.9,5.928C8.91,5.428 8.501,5 8,5ZM8.002,11C7.45,11 7,11.45 7,12C7,12.55 7.45,13 8.002,13C8.554,13 9,12.55 9,12C9,11.45 8.554,11 8.002,11Z");
                IconColor = Brushes.Goldenrod;
                break;

            //case MessageDialogIconKind.Question:
            //    IconData = Geometry.Parse("M10.5,18H13.5V15H10.5V18M12,2A10,10 0 1,0 22,12A10,10 0 0,0 12,2M12,6A4,4 0 0,1 16,10C16,12.5 13,13 13,15H11C11,12 14,11.5 14,10A2,2 0 0,0 12,8A2,2 0 0,0 10,10H8A4,4 0 0,1 12,6Z"); // Question (?)
            //    IconColor = Brushes.DarkCyan;
            //    break;
            case MessageDialogIconKind.Question:
                IconData = Geometry.Parse("M10 20A10 10 0 1 1 10 0A10 10 0 0 1 10 20ZM12 7C12 7.28 11.79 7.8 11.58 8L10 9.58C9.43 10.16 9 11.18 9 12V13H11V12C11 11.71 11.21 11.2 11.42 11L13 9.42C13.57 8.84 14 7.82 14 7A4 4 0 1 0 6 7H8A2 2 0 1 1 12 7ZM9 15V17H11V15H9Z");
                IconColor = Brushes.DarkCyan;
                break;

            default:
                IconData = null;
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


//public class HalfConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if (value is double d and > 0)
//            return d * 0.8; 
//        throw new Exception($"Unable to get value for {nameof(HalfConverter)}");
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        => throw new NotImplementedException();
//}
