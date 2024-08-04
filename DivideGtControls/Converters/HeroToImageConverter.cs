using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DivideGT;

namespace DivideGtCommons
{
    public class HeroToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value == null)
                return "/DivideGtCommons;component/Heroes/Icons/AiSR.png";
            var hero = (HeroEnum)value;
            if ((int)hero == 0 || (int)hero == 1 || (int)hero == 2)
                hero = HeroEnum.Unknown;
            return $"/DivideGtCommons;component/Heroes/Icons/{(hero.ToString().EndsWith("T") ? hero.ToString() : (hero.ToString() + "SR"))}.png";

        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
