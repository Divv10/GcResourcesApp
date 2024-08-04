using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class GearSlotCost : NotifyPropertyChanged
    {
        private int _raidMatsCost;
        public int RaidMatsCost
        {
            get => _raidMatsCost;
            set => SetProperty(ref _raidMatsCost, value);
        }

        private int _blueStonesCost;
        public int BlueStonesCost
        {
            get => _blueStonesCost;
            set => SetProperty(ref _blueStonesCost, value);
        }
    }
}
