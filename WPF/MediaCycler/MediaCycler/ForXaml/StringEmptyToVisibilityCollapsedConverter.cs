using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MediaCycler.Utils;

namespace MediaCycler.ForXaml
{
    public class StringEmptyToVisibilityCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var sValue = (string)value;
                return sValue.IsBlank() ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}