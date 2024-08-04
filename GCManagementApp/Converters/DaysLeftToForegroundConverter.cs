using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    internal class DaysLeftToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value > 0 ? new SolidColorBrush(Colors.Orange) : new SolidColorBrush(Colors.Green);
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
