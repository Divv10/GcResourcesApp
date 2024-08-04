using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class DaysLeftToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value > 0 ? string.Format(Properties.Resources.XDays, Math.Ceiling((double)value)) : Properties.Resources.Ready;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
