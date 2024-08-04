using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class HeroBuild : NotifyPropertyChanged
    {
        private string[] _limitBreaks;
        public string[] LimitBreaks
        {
            get => _limitBreaks;
            set => SetProperty(ref _limitBreaks, value);
        }

        private List<GearSet> _sets;
        public List<GearSet> Sets
        {
            get => _sets; 
            set => SetProperty(ref _sets, value);
        }

        private Dictionary<ChaserTraitEnum, int> _csTraits;
        public Dictionary<ChaserTraitEnum, int> CsTraits
        {
            get => _csTraits; 
            set => SetProperty(ref _csTraits, value);
        }

        private string[] _accessories;
        public string[] Accessories
        {
            get => _accessories;
            set => SetProperty(ref _accessories, value);
        }
    }
}
