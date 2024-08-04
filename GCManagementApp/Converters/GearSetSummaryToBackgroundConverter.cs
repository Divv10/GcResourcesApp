using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace GCManagementApp.Converters
{
    public class GearSetSummaryToBackgroundConverter : IValueConverter
    {
        private static LinearGradientBrush gradientBrush { get; } = new LinearGradientBrush() 
        { 
            StartPoint = new System.Windows.Point(0, 0),
            EndPoint = new System.Windows.Point(1, 1),
            GradientStops = new GradientStopCollection() 
            {
                new GradientStop(Colors.Orange, 0.0),
                new GradientStop(Colors.Red, 0.2),
                new GradientStop(Colors.MediumOrchid, 0.4),
                new GradientStop(Colors.LimeGreen, 0.8),
                new GradientStop(Colors.DeepSkyBlue, 1.0),
            } 
        };

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;

            if (stringValue == "Mixed")
                return gradientBrush;
            if (stringValue == GearSet.Purple.GetDescription())
                return Application.Current.Resources["EqPurpleSet"];
            if (stringValue == GearSet.Green.GetDescription())
                return Application.Current.Resources["EqGreenSet"];
            if (stringValue == GearSet.Orange.GetDescription())
                return Application.Current.Resources["EqOrangeSet"];
            if (stringValue == GearSet.Blue.GetDescription())
                return Application.Current.Resources["EqBlueSet"];
            if (stringValue == GearSet.Red.GetDescription())
                return Application.Current.Resources["EqRedSet"];
            return Brushes.LightGray;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
