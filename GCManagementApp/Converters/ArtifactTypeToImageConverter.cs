using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class ArtifactTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string artifactType)
            {
                artifactType = artifactType.Replace(" ", "");
                return $"/GCManagementApp;component/Resources/Artifact/{artifactType}.png";
            }
            return "/GCManagementApp;component/Resources/Artifacts/Normal.png";
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
