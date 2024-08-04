using GCManagementApp.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    public class GearSetToFillConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = int.Parse((string)parameter);
            GearSet myEnum = (GearSet)value;
            switch (myEnum)
            {
                default: return Brushes.Transparent;
                case GearSet.Purple: return Application.Current.Resources["EqPurpleSet"];
                case GearSet.Green: return Application.Current.Resources["EqGreenSet"];
                case GearSet.Orange: return Application.Current.Resources["EqOrangeSet"];
                case GearSet.Blue: return Application.Current.Resources["EqBlueSet"];
                case GearSet.Red: return Application.Current.Resources["EqRedSet"];
                case GearSet.RedBlue: return id >= 2 ? Application.Current.Resources["EqBlueSet"] : Application.Current.Resources["EqRedSet"];
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
