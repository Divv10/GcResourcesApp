using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    internal class SILevelToMaximumTraitsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                return 0d;

            var siLevel = (int)values[0];
            var isCoreOpen = (bool)values[1];

            return AvailableSiTraits.MaximumTraits(siLevel, isCoreOpen);
        }


        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
