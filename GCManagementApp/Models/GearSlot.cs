using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class GearSlot : NotifyPropertyChanged
    {
        private GearSlotRankEnum _rank;
        public GearSlotRankEnum Rank
        {
            get => _rank;
            set => SetProperty(ref _rank, value);
        }

        private int _upgradeLevel;
        public int UpgradeLevel
        {
            get => _upgradeLevel;
            set => SetProperty(ref _upgradeLevel, value);
        }

        public GearSlot()
        {
            Rank = GearSlotRankEnum.Normal;
            UpgradeLevel = 0;
        }
    }
}
