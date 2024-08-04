using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class RecommendedEquipment : NotifyPropertyChanged
    {
        private GearSet _setColor;
        public GearSet SetColor
        {
            get => _setColor;
            set => SetProperty(ref _setColor, value);
        }

        private string _subStat;
        public string SubStat
        {
            get => _subStat;
            set => SetProperty(ref _subStat, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
    }
}
