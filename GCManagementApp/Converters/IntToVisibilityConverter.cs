using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    internal class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int number)
            {
                return number == 0 ? Visibility.Hidden : Visibility.Visible;
            }

            return Visibility.Hidden;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
