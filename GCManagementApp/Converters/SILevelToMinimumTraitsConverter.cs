using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    internal class SILevelToMinimumTraitsConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value[0] == DependencyProperty.UnsetValue || value[1] == DependencyProperty.UnsetValue)
            {
                return 0d;
            }
            var x = System.Convert.ToInt32(value[0]);
            var y = System.Convert.ToDouble(value[1]);
            return Math.Max(AvailableSiTraits.MinimumTraits(x), y);
        }


        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
