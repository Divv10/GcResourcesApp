using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    internal class AccessorySetToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is AccessorySetEnum tier)
            {
                return tier == AccessorySetEnum.None ? Visibility.Hidden : Visibility.Visible;
            }

            return Visibility.Hidden;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
