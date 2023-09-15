using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace MediaCycler2.ForXaml
{
    public class BoolToVisibilityCollapsedReversedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var bValue = !(bool)value;
                return bValue ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
