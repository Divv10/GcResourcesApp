using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace GCManagementApp.Converters
{
    internal class ElementToColorConverter : IValueConverter
    {
        private SolidColorBrush Gray { get; } = new SolidColorBrush(Colors.Gray);
        private SolidColorBrush Blue { get; } = new SolidColorBrush(Color.FromArgb(255, 152, 182, 234));
        private SolidColorBrush Red { get; } = new SolidColorBrush(Color.FromArgb(255, 224, 102, 102));
        private SolidColorBrush Green { get; } = new SolidColorBrush(Color.FromArgb(255, 106, 168, 79));

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ContentAttributeEnum content)
            {
                switch (content)
                {
                    case ContentAttributeEnum.None: return Gray;
                    case ContentAttributeEnum.Blue: return Blue;
                    case ContentAttributeEnum.Red: return Red;
                    case ContentAttributeEnum.Green: return Green;
                }
                return Gray;
            }
            return Gray;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
