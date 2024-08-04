using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class GearSlotTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is WeaponTypeEnum type)
            {
                switch (value)
                {
                    case WeaponTypeEnum.Weapon:
                        return "/GCManagementApp;component/Resources/Gear/weapon.png";
                    case WeaponTypeEnum.Armor:
                        return "/GCManagementApp;component/Resources/Gear/armor.png";
                    case WeaponTypeEnum.SubWeapon:
                        return "/GCManagementApp;component/Resources/Gear/subweapon.png";
                    default:
                        return "/GCManagementApp;component/Resources/Gear/subarmor.png";
                }    
            }
            return "/GCManagementApp;component/Resources/Gear/weapon.png";
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
