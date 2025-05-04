using System;
using System.Globalization;
using Avalonia.Data.Converters;
using FluentAvalonia.UI.Controls;
using UiTools.Av.ViewModels;

namespace UiTools.Av.Converters;

public class StackableMessageSeverityToInfoBarSeverityConverter : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is StackableMessageSeverity sms)
        {
            return Enum.Parse<InfoBarSeverity>(sms.ToString());
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new Exception($"{nameof(StackableMessageSeverityToInfoBarSeverityConverter)}.{nameof(ConvertBack)} not implemented");
    }
}