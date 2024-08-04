using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    internal class AccessorySetToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is AccessorySetEnum tier)
            {
                switch (tier)
                {
                    case AccessorySetEnum.Purple:
                        return new SolidColorBrush(Color.FromArgb(255, 156, 62, 151));
                    case AccessorySetEnum.Orange:
                        return new SolidColorBrush(Color.FromArgb(255, 198, 124, 57));
                    case AccessorySetEnum.Blue:
                        return new SolidColorBrush(Color.FromArgb(255, 90, 191, 207));
                    default:
                        return new SolidColorBrush(Colors.Gray);
                }
            }

            return new SolidColorBrush(Colors.Gray);
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
