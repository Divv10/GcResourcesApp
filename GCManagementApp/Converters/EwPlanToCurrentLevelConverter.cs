using GCManagementApp.Enums;
using GCManagementApp.Models;
using GCManagementApp.Static;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    internal class EwPlanToCurrentLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is EwPlan ewPlan)
            {
                if (!ewPlan.IsEwOwned)
                    return Properties.Resources.None;
                return $"+{ewPlan.CurrentLevel}";
            }

            return Properties.Resources.None;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
