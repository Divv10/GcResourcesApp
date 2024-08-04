using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class AttributeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is HeroAttribute attribute)
            {
                return $"/GCManagementApp;component/Resources/Attributes/{attribute}.png";
            }
            return "/GCManagementApp;component/Resources/Attributes/yellow.png";
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
