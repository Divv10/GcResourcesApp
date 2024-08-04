using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    internal class GearSlotRankToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is GearSlotRankEnum rank)
            {
                switch (rank)
                {
                    case GearSlotRankEnum.Normal:
                        return new SolidColorBrush(Color.FromArgb(255, 231, 237, 241)); //#E7EDF1
                    case GearSlotRankEnum.Premium:
                        return new SolidColorBrush(Color.FromArgb(255, 242, 207, 68)); //#F2CF44
                    case GearSlotRankEnum.Rare:
                        return new SolidColorBrush(Color.FromArgb(255, 78, 172, 237)); //#4EACED
                    case GearSlotRankEnum.Unique:
                        return new SolidColorBrush(Color.FromArgb(255, 175, 91, 240)); //#AF5BF0
                    default:
                        return new SolidColorBrush(Colors.Gray);
                }
            }

            return new SolidColorBrush(Colors.Gray);
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
