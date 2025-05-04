using Microsoft.UI.Xaml.Data;
using UiTools.Uno.My.ViewModels;

namespace UiTools.Uno.My.Converters
{
    public class StackableMessageSeverityToInfoBarSeverityConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            
            if (value is StackableMessageSeverity sms)
            {
                return Enum.Parse<InfoBarSeverity>(sms.ToString());
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new Exception($"{nameof(StackableMessageSeverityToInfoBarSeverityConverter)}.{nameof(ConvertBack)} not implemented");
        }

    }

}