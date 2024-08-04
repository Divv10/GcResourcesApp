using GCManagementApp.Enums;
using GCManagementApp.Models;
using GCManagementApp.Static;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class SiLevelToCoreOpenVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                0 => Visibility.Visible,
                5 => Visibility.Visible,
                10 => Visibility.Visible,
                _ => Visibility.Hidden,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
