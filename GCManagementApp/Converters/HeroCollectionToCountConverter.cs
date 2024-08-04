using GCManagementApp.Enums;
using GCManagementApp.Models;
using GCManagementApp.Static;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace GCManagementApp.Converters
{
    internal class HeroCollectionToCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ObservableCollection<HeroGrowth> items)
            {
                return items.Count;
            }

            return 0;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
