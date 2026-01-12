using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Models;
using GCManagementApp.Static;
using System;
using System.Globalization;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class AccessoryToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 3)
                return AccessorySetEnum.None.GetDescription();

            try
            {
                AccessoryTierEnum tier = (AccessoryTierEnum)values[0];
                int level = (int)values[1];
                AccessorySetEnum set = (AccessorySetEnum)values[2];

                return set == AccessorySetEnum.None ? AccessorySetEnum.None.GetDescription() : $"{tier} +{level}";
            }
            catch 
            {
                return null;
            }
        }

        public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
