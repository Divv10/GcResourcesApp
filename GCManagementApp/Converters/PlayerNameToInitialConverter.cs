using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class PlayerNameToInitialConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string playername)
            {
                return playername?.Substring(0, 1) ?? "GC";
            }
            return "GC";
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
