using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro;

namespace petratracker.Utility
{

    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var purpose = System.Convert.ToString(parameter);
            switch (purpose)
            {
                case "ConvertToAccentColor":
                    var aceentColorName = System.Convert.ToString(value);
                    var accent = ThemeManager.Accents.FirstOrDefault(a => string.CompareOrdinal(a.Name, aceentColorName) == 0);
                    if (null != accent)
                        return accent.Resources["AccentColorBrush"] as Brush;
                    break;
                case "ConvertToBaseColor":
                    var baseColorName = System.Convert.ToString(value);
                    var converter = new BrushConverter();
                    if (string.CompareOrdinal(baseColorName, "BaseLight") == 0)
                    {
                        var brush = (Brush)converter.ConvertFromString("#FFFFFFFF");
                        return brush;
                    }
                    if (string.CompareOrdinal(baseColorName, "BaseDark") == 0)
                    {
                        var brush = (Brush)converter.ConvertFromString("#FF000000");
                        return brush;
                    }

                    break;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DecimalToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int num = (int)value;
            return num;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int month = (int) value;
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
