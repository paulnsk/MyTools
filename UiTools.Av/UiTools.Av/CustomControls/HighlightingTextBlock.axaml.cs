using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Globalization;

namespace UiTools.Av.CustomControls;


/*
 * the version below works but has a weird issue which causes empty text (of correct size) to be drawn instead of the actial text
 * to be removed
 */

//public partial class HighlightingTextBlock : UserControl
//{

//    public static readonly StyledProperty<string> TextProperty =
//        AvaloniaProperty.Register<HighlightingTextBlock, string>(nameof(Text));

//    public string Text
//    {
//        get => GetValue(TextProperty);
//        set => SetValue(TextProperty, value);
//    }

//    public static readonly StyledProperty<string> HighlightTextProperty =
//        AvaloniaProperty.Register<HighlightingTextBlock, string>(nameof(HighlightText));

//    public string HighlightText
//    {
//        get => GetValue(HighlightTextProperty);
//        set => SetValue(HighlightTextProperty, value);
//    }

//    public static readonly StyledProperty<IBrush> HighlightBrushProperty =
//        AvaloniaProperty.Register<HighlightingTextBlock, IBrush>(nameof(HighlightBrush), Brushes.DarkOrange);

//    public IBrush HighlightBrush
//    {
//        get => GetValue(HighlightBrushProperty);
//        set => SetValue(HighlightBrushProperty, value);
//    }

//    public HighlightingTextBlock()
//    {
//        InitializeComponent();
//    }

//    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
//    {
//        base.OnPropertyChanged(change);

//        UpdateInlines();

//        //if (change.Property == TextProperty ||
//        //    change.Property == HighlightTextProperty ||
//        //    change.Property == HighlightBrushProperty)
//        //{
//        //    UpdateInlines();
//        //}
//    }

//    private void UpdateInlines()
//    {
//        if (PartTextBlock == null) return;

//        var fullText = Text;
//        var highlight = HighlightText;

//        PartTextBlock.Inlines!.Clear();


//        if (string.IsNullOrEmpty(fullText))
//        {
//            return;
//        }

//        if (string.IsNullOrEmpty(highlight))
//        {
//            PartTextBlock.Inlines.Add(new Run(fullText));
//            return;
//        }

//        var currentIndex = 0;
//        while (currentIndex < fullText.Length)
//        {
//            var index = fullText.IndexOf(highlight, currentIndex, StringComparison.OrdinalIgnoreCase);

//            if (index == -1)
//            {
//                PartTextBlock.Inlines.Add(new Run(fullText.Substring(currentIndex)));
//                break;
//            }

//            // Add text before the highlight
//            if (index > currentIndex)
//            {
//                PartTextBlock.Inlines.Add(new Run(fullText.Substring(currentIndex, index - currentIndex)));
//            }

//            // Add highlighted text
//            PartTextBlock.Inlines.Add(new Run(fullText.Substring(index, highlight.Length))
//            {
//                Background = HighlightBrush
//            });

//            currentIndex = index + highlight.Length;
//        }
//    }
//}




public partial class HighlightingTextBlock : Control
{
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<HighlightingTextBlock, string?>(nameof(Text));

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<string?> HighlightTextProperty =
        AvaloniaProperty.Register<HighlightingTextBlock, string?>(nameof(HighlightText));

    public string? HighlightText
    {
        get => GetValue(HighlightTextProperty);
        set => SetValue(HighlightTextProperty, value);
    }

    public static readonly StyledProperty<IBrush?> HighlightBrushProperty =
        AvaloniaProperty.Register<HighlightingTextBlock, IBrush?>(nameof(HighlightBrush), Brushes.DarkOrange);

    public IBrush? HighlightBrush
    {
        get => GetValue(HighlightBrushProperty);
        set => SetValue(HighlightBrushProperty, value);
    }
    
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        TextBlock.ForegroundProperty.AddOwner<HighlightingTextBlock>();

    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public static readonly StyledProperty<double> FontSizeProperty =
        TextBlock.FontSizeProperty.AddOwner<HighlightingTextBlock>();

    public double FontSize
    {
        get => GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly StyledProperty<FontFamily> FontFamilyProperty =
        TextBlock.FontFamilyProperty.AddOwner<HighlightingTextBlock>();

    public FontFamily FontFamily
    {
        get => GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly StyledProperty<FontWeight> FontWeightProperty =
        TextBlock.FontWeightProperty.AddOwner<HighlightingTextBlock>();

    public FontWeight FontWeight
    {
        get => GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    public static readonly StyledProperty<FontStyle> FontStyleProperty =
        TextBlock.FontStyleProperty.AddOwner<HighlightingTextBlock>();

    public FontStyle FontStyle
    {
        get => GetValue(FontStyleProperty);
        set => SetValue(FontStyleProperty, value);
    }

    static HighlightingTextBlock()
    {
        AffectsMeasure<HighlightingTextBlock>(
            TextProperty, HighlightTextProperty, ForegroundProperty, FontSizeProperty,
            FontFamilyProperty, FontWeightProperty, FontStyleProperty);

        AffectsRender<HighlightingTextBlock>(
            TextProperty, HighlightTextProperty, HighlightBrushProperty, ForegroundProperty,
            FontSizeProperty, FontFamilyProperty, FontWeightProperty, FontStyleProperty);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var text = Text;
        if (string.IsNullOrEmpty(text))
            return;

        var formattedText = CreateFormattedText(text, Bounds.Size);
        if (formattedText == null)
            return;

        var highlight = HighlightText;
        if (!string.IsNullOrEmpty(highlight) && HighlightBrush != null)
        {
            var currentIndex = 0;
            while (currentIndex < text.Length)
            {
                var index = text.IndexOf(highlight, currentIndex, StringComparison.OrdinalIgnoreCase);
                if (index == -1)
                    break;

                var highlightGeometry = formattedText.BuildHighlightGeometry(new Point(), index, highlight.Length);

                context.DrawGeometry(HighlightBrush, null, highlightGeometry);


                currentIndex = index + highlight.Length;
            }
        }
        
        context.DrawText(formattedText, new Point(0, 0));
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var text = Text;
        if (string.IsNullOrEmpty(text))
        {
            return new Size();
        }

        var formattedText = CreateFormattedText(text, availableSize);
        return formattedText == null ? new Size() : new Size(formattedText.Width, formattedText.Height);
    }

    private FormattedText? CreateFormattedText(string text, Size constraint)
    {
        if (string.IsNullOrEmpty(text) || FontSize <= 0)
            return null;
        
        return new FormattedText(
            text,
            CultureInfo.CurrentUICulture,
            FlowDirection.LeftToRight,
            new Typeface(FontFamily, FontStyle, FontWeight),
            FontSize,
            Foreground)
        {
            MaxTextWidth = constraint.Width,
            MaxTextHeight = constraint.Height,
            Trimming = TextTrimming.CharacterEllipsis
        };
    }
}