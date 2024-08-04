using GCManagementApp.Enums;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GCManagementApp.Models
{
    public class ContentTeam : NotifyPropertyChanged
    {
        private ObservableCollection<HeroGrowth> _heroes;
        public ObservableCollection<HeroGrowth> Heroes
        {
            get => _heroes;
            set => SetProperty(ref _heroes, value);
        }

        private ContentAttributeEnum _element;
        public ContentAttributeEnum Element
        {
            get => _element;
            set => SetProperty(ref _element, value);
        }

        public ContentTeam()
        {
            Heroes = new ObservableCollection<HeroGrowth>();
        }
    }
}
