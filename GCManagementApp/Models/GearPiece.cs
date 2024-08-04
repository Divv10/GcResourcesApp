using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class GearPiece : NotifyPropertyChanged
    {
        private GearTier _tier;
        public GearTier Tier 
        {
            get => _tier;
            set => SetProperty(ref _tier, value);
        }

        public GearPiece()
        {
            Tier = GearTier.None;
        }
    }
}
