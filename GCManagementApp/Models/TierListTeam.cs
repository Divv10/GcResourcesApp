using GCManagementApp.Enums;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GCManagementApp.Models
{
    public class TierListTeam : NotifyPropertyChanged
    {
        private ObservableCollection<Hero> _heroes;
        public ObservableCollection<Hero> Heroes
        {
            get => _heroes;
            set => SetProperty(ref _heroes, value);
        }

        private string _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private SolidColorBrush _labelColor;
        [JsonIgnore]
        [XmlIgnore]
        public SolidColorBrush LabelColor
        {
            get => _labelColor;
            set => SetProperty(ref _labelColor, value);
        }

        private string _labelColorHex;
        public string LabelColorHex
        {
            get => _labelColorHex;
            set => SetProperty(ref _labelColorHex, value);
        }

        public TierListTeam()
        {
            Heroes = new ObservableCollection<Hero>();
        }
    }
}
