using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class RecommendedBuild : NotifyPropertyChanged
    {
        private List<ContentKey> _contentKeys;
        public List<ContentKey> ContentKeys
        {
            get => _contentKeys;
            set => SetProperty(ref _contentKeys, value);
        }

        private List<RecommendedEquipment> _recommendedEquips;
        public List<RecommendedEquipment> RecommendedEquips
        {
            get => _recommendedEquips;
            set => SetProperty(ref _recommendedEquips, value);
        }

        private HeroBuild _heroBuild;
        public HeroBuild HeroBuild
        {
            get => _heroBuild; 
            set => SetProperty(ref _heroBuild, value);
        }
    }
}
