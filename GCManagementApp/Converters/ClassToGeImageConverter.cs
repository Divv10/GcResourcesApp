using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class ClassToGeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is HeroClass className)
            {
                return $"/GCManagementApp;component/Resources/Materials/GrowthEssence{className}.png";
            }
            return "/GCManagementApp;component/Resources/Materials/GrowthEssenceAssault.png";
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
