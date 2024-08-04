using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class LevelToProgressValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter is GrowthLevelType growthLevelType)
            {
                switch (parameter)
                {
                    default:
                        return 0;
                    case GrowthLevelType.Level:
                        return 100 * (int)value / (int)StaticValues.MaxLevel;
                    case GrowthLevelType.SoulImprint:
                        return 100 * (int)value / (int)StaticValues.MaxSiLevel;
                    case GrowthLevelType.Chaser:
                        return 100 * (int)value / (int)StaticValues.MaxClLevel;
                    case GrowthLevelType.Transcendence:
                        return 100 * (int)value / (int)StaticValues.MaxTranscendenceLevel;
                    case GrowthLevelType.ExclusiveWeapon:
                        return 100 * (int)value / (int)StaticValues.MaxExclusiveWeaponUpgrade;
                    case GrowthLevelType.Pet:
                        return 100 * (int)value / (int)StaticValues.MaxPetLevel;
                }
            }
            return 0;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
