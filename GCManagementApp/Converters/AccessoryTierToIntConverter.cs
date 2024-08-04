using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class AccessoryTierToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is AccessoryTierEnum tier)
                return (int)tier;

            return 0;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (AccessoryTierEnum)value;
        }
    }
}
