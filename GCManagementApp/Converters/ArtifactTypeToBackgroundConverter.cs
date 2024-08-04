using GCManagementApp.Enums;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    public class ArtifactTypeToBackgroundConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return Brushes.Transparent;

            ArtifactType myEnum = (ArtifactType)value;
            switch (myEnum)
            {
                default: return Brushes.Transparent;
                case ArtifactType.Normal: return Brushes.LightGray;
                case ArtifactType.Frozen: return Brushes.DeepSkyBlue;
                case ArtifactType.Burning: return Brushes.Orange;
                case ArtifactType.Cursed: return Brushes.MediumOrchid;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
