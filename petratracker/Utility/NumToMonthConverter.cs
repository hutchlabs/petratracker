using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro;

namespace petratracker.Utility
{
    public class NumToMonthConverter : IValueConverter
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
