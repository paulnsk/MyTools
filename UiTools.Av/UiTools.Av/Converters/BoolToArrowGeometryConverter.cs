using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiTools.Av.Converters;

public class BoolToArrowGeometryConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isDescending)
        {
            // Геометрия для стрелки вверх (оригинальный SVG)
            var upArrow = new GeometryGroup
            {
                Children =
                    {
                        // Путь 1: Основная стрелка
                        Geometry.Parse("M399.436,191.249L262.903,3.516C261.298,1.306,258.738,0,255.999,0c-2.731,0-5.291,1.306-6.895,3.516L112.571,191.249c-1.084,1.485-1.638,3.251-1.638,5.018c0,1.323,0.316,2.645,0.939,3.874c1.451,2.859,4.386,4.659,7.595,4.659h85.333v93.867c0,4.719,3.823,8.533,8.533,8.533h85.333c4.719,0,8.533-3.814,8.533-8.533V204.8h85.333c3.209,0,6.153-1.8,7.603-4.659C401.595,197.291,401.322,193.852,399.436,191.249z"),
                        // Путь 2: Первая полоска
                        Geometry.Parse("M281.603,324.266h-51.2c-14.114,0-25.6,11.486-25.6,25.6s11.486,25.6,25.6,25.6h51.2c14.114,0,25.6-11.486,25.6-25.6S295.717,324.266,281.603,324.266z"),
                        // Путь 3: Вторая полоска
                        Geometry.Parse("M281.603,392.532h-51.2c-14.114,0-25.6,11.486-25.6,25.6s11.486,25.6,25.6,25.6h51.2c14.114,0,25.6-11.486,25.6-25.6S295.717,392.532,281.603,392.532z"),
                        // Путь 4: Третья полоска
                        Geometry.Parse("M281.603,460.799h-51.2c-14.114,0-25.6,11.486-25.6,25.6s11.486,25.6,25.6,25.6h51.2c14.114,0,25.6-11.486,25.6-25.6S295.717,460.799,281.603,460.799z")
                    }
            };

            // Геометрия для стрелки вниз (поворот на 180 градусов)
            var downArrow = new GeometryGroup
            {
                Children = upArrow.Children, // Копируем те же пути
                Transform = new RotateTransform(180, 256, 256) // Поворот вокруг центра (половина viewBox: 511.999/2)
            };

            return isDescending ? downArrow : upArrow;
        }

        return null; // Fallback
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("ConvertBack is not supported.");
    }
}